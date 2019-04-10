using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebClient.Core.Entities;
using WebClient.Core.Messages;
using WebClient.Core.ViewModels;
using WebClient.Repositories.Interfaces;
using WebClient.Services.Interfaces;

namespace WebClient.Services.Implements
{
    public class DepartmentService : IDepartmentService
    {
        private IDepartmentRepository departmentRepository;
        private IEmployeeRepository employeeRepository;
        private IRabbitMQService rabbitMQService;
        private readonly IMapper mapper;
        public DepartmentService(
            IDepartmentRepository departmentRepository, 
            IEmployeeRepository employeeRepository, 
            IRabbitMQService rabbitMQService,
            IMapper mapper)
        {
            this.departmentRepository = departmentRepository;
            this.employeeRepository = employeeRepository;
            this.rabbitMQService = rabbitMQService;
            this.mapper = mapper;
        }

        /// <summary>
        /// Id of the Department that will be deleted
        /// </summary>
        /// <param name="idDonvi">Id of Department</param>
        /// <param name="handler">Handler id</param>
        /// <returns>return 0: exist department children and employees
        ///          return 1: exist department children
        ///          return 2: exist employees
        ///          return 3: delete success
        /// </returns>
        public async Task<Department> DeleteDepartmentAsync(string departmentCode, int handler)
        {
            var department = await this.departmentRepository.GetDepartmentByCode(departmentCode);
            if (department == null)
            {
                throw new Exception("Đơn vị này không tồn tại");
            }

            var children = await this.departmentRepository.GetDepartmentsByIdParent(department.Id_DonVi);
            if (children != null && children.Count() > 0)
            {
                throw new Exception("Đơn vị này đang có các đơn vị con");
            }

            var employees = await this.employeeRepository.GetEmployeesByDeparmentId(department.Id_DonVi);
            if (employees != null && children.Count() > 0)
            {
                throw new Exception("Đơn vị này đang có nhân viên");
            }

            department.Tinh_Trang = 0;
            department.Id_NV_CapNhat = handler;
            department.Ngay_CapNhat = DateTime.Now;

            await this.departmentRepository.UpdateAsync(department);
            await this.PublishUpdatingDepartment(department.Ma_DonVi, department);
            return department;
        }

        /// <summary>
        /// Get all Departments
        /// </summary>
        /// <returns>List Departments</returns>
        public async Task<IEnumerable<Department>> GetAllAsync()
        {
            var list = await this.departmentRepository.GetAllAsync();
            return list;
        }

        /// <summary>
        /// Get tree nodes that are children of the department
        /// </summary>
        /// <param name="departmentId">Id of department</param>
        /// <returns>Tree nodes</returns>
        public async Task<IEnumerable<TreeNode>> GetTreeNodeChildrenOfDepartment(int departmentId, int accountId)
        {
            var result = Enumerable.Empty<TreeNode>();
            var departments = await this.departmentRepository.GetDepartmentsByIdParent(departmentId, accountId);
            result = departments.Select(x => new TreeNode
            {
                Id = "D" + x.Id_DonVi,
                Children = true,
                Text = x.Ten_DonVi,
                TypeNode = "Deparment"
            });

            var employees = await this.employeeRepository.GetEmployeesByDeparmentId(departmentId);

            result = result.Concat(employees.Select(x => new TreeNode
            {
                Id = "E" + x.Id_NhanVien,
                Children = true,
                Text = x.Ho_Ten,
                TypeNode = "Employee"
            }));

            return result;
        }

        /// <summary>
        /// Get department by id
        /// </summary
        /// <param name="id">The id of department</param>
        /// <returns>The department</returns>
        public async Task<Department> GetDepartmentById(int id)
        {
            return await this.departmentRepository.GetByIdAsync(id, true);
        }

        /// <summary>
        /// Save department
        /// </summary>
        /// <param name="departmentVM">Department VM</param>
        /// <param name="handler">Handler ID</param>
        /// <returns>Department instance</returns>
        public async Task<Department> SaveDepartment(DepartmentVM departmentVM, int handler)
        {
            Department entity = mapper.Map<Department>(departmentVM);
            var parent = await this.departmentRepository.GetDepartmentByCode(departmentVM.Ma_DV_Cha);
            if (parent != null)
            {
                entity.Id_DV_Cha = parent.Id_DonVi;
            }

            if (!string.IsNullOrEmpty(departmentVM.Ma_DonVi))
            {
                // Edit
                var department = await this.departmentRepository.GetDepartmentByCode(departmentVM.Ma_DonVi);
                if (entity == null)
                {
                    throw new Exception("Đơn vị này không tồn tại");
                }

                entity.Ngay_CapNhat = DateTime.Now;
                entity.Id_NV_CapNhat = handler;
                entity.Id_DonVi = department.Id_DonVi;

                entity = await this.departmentRepository.UpdateDepartmentAsync(entity);
                await this.PublishUpdatingDepartment(entity.Ma_DonVi, entity);
            }
            else
            {
                entity.Ma_DonVi = Guid.NewGuid().ToString();
                entity.Ngay_KhoiTao = DateTime.Now;
                entity.Tinh_Trang = 1;
                entity.Id_NV_KhoiTao = handler;

                entity = await this.departmentRepository.AddDepartmentAsync(entity);
                await this.PublishCreatingDepartment(entity);
            }

            return entity;
        }

        /// <summary>
        /// convert jstree
        /// </summary>
        /// <param name="list">the list departments</param>
        /// <returns>list departments converted</returns>
        public IEnumerable<Department> ConvertToTree(IEnumerable<Department> list, int idRoot = 0)
        {
            if (list != null && list.Count() > 0)
            {
                if (list.Count() == 1)
                {
                    return list;
                }
                // groups features by Id feature parent
                var groupDepartments = list.GroupBy(x => x.Id_DV_Cha);
                Department root = new Department() { Id_DV_Cha = 0 };

                foreach (var gr in groupDepartments)
                {
                    Department fr = list.FirstOrDefault(y => y.Id_DonVi == gr.Key);
                    if (fr != null)
                    {
                        if (fr.Id_DonVi == idRoot)
                        {
                            root = fr;
                        }

                        fr.Children = gr;
                    }
                };
                // Gets root features that has ID_CN_PR = 0

                return new List<Department> { root };
            }
            return null;
        }

        /// <summary>
        /// get departmentVM by idonvi
        /// </summary>
        /// <param name="idDonVi">the iddonvi</param>
        /// <returns>department vm</returns>
        public async Task<DepartmentVM> GetDeparmentVMByID(int idDonVi)
        {
            var department = await departmentRepository.GetDepartmentByIdAsync(idDonVi);
            var departmnetVM = mapper.Map<DepartmentVM>(department);
            return departmnetVM;
        }

        /// <summary>
        /// Get departments with condition: to idDepartmentStart from idDepartmentEnd.
        /// </summary>
        /// <param name="idDepartment">id department start</param>
        /// <param name="accountId">Account Id whose user is performing this action</param>
        /// <param name="idDepartmentEnd">id department End. Default = -1, if idDepartmentEnd = -1 then return all.</param>
        /// <returns>list departments </returns>
        public async Task<IEnumerable<Department>> GetTreeDepartmentsdWithTerm(int accountId, int idDepartmentStart, int idDepartmentEnd = -1)
        {
            var departments = await this.departmentRepository.GetDepartmentsWithTerm(accountId, idDepartmentStart, idDepartmentEnd);
            return this.ConvertToTree(departments, idDepartmentStart);
        }

        /// <summary>
        /// update email
        /// </summary>
        /// <param name="emailDepartmentVM">the email of department vm</param>
        /// <param name="handler">the employee id current</param>
        /// <param name="departmentId">the department id current</param>
        /// <returns>the task</returns>
        public async Task<Department> UpdateEmail(EmailDepartmentVM emailDepartmentVM, int handler, int departmentId)
        {
            var department = await this.departmentRepository.GetDepartmentByCode(emailDepartmentVM.Ma_DonVi);
            if (department != null)
            {
                department.Email = emailDepartmentVM.Email;
                department.SMTP_Email = emailDepartmentVM.SMTP_Email;
                department.Port_Email = emailDepartmentVM.Port_Email;
                department.Account_Email = emailDepartmentVM.Account_Email;
                department.Pass_Email = emailDepartmentVM.Pass_Email;
                department.Id_NV_CapNhat = handler;
                department = await this.departmentRepository.UpdateEmail(department);

                await this.PublishUpdatingDepartment(department.Ma_DonVi, department);
            }

            return department;
        }

        /// <summary>
        /// Gets children that are controlled by the account by parent Id
        /// </summary>
        /// <param name="parentId">parent department id</param>
        /// <param name="accountId">Account id</param>
        /// <returns>List department</returns>
        public async Task<IEnumerable<Department>> GetChildrenByParentId(int parentId, int accountId)
        {
            var departments = await this.departmentRepository.GetDepartmentsByIdParent(parentId, accountId);
            return departments;
        }

        /// <summary>
        /// Gets all children by parent Id
        /// </summary>
        /// <param name="parentId">Parent department id</param>
        /// <returns>List department</returns>
        public async Task<IEnumerable<Department>> GetChildrenByParentId(int parentId)
        {
            var departments = await this.departmentRepository.GetDepartmentsByIdParent(parentId);
            return departments;
        }

        /// <summary>
        /// Gets id departments that managed by the account
        /// </summary>
        /// <param name="accountId">Account id</param>
        /// <returns>List id</returns>
        public async Task<IEnumerable<int>> GetControlledDepartmentIds(int accountId)
        {
            var list = await this.departmentRepository.GetControlledDepartments(accountId);

            return list.Select(x => x.Id_DonVi);
        }

        private async Task PublishUpdatingDepartment(string oldDepartmentCode, Department department)
        {
            var parent = await this.departmentRepository.GetDepartmentByIdAsync(department.Id_DV_Cha);
            var message = this.mapper.Map<DepartmentMessage>(department);
            if (parent != null)
            {
                message.Ma_DV_Cha = parent.Ma_DonVi;
            }

            message.Ma_DV_Cu = oldDepartmentCode;

            rabbitMQService.Publish(message, RabbitActionTypes.Update, RabbitEntities.Department);
        }

        private async Task PublishCreatingDepartment(Department department)
        {
            var parent = await this.departmentRepository.GetDepartmentByIdAsync(department.Id_DV_Cha);
            var message = this.mapper.Map<DepartmentMessage>(department);
            if (parent != null)
            {
                message.Ma_DV_Cha = parent.Ma_DonVi;
            }

            rabbitMQService.Publish(message, RabbitActionTypes.Create, RabbitEntities.Department);
        }
    }
}
