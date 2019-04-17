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
        /// Save department
        /// </summary>
        /// <param name="departmentVM">Department VM</param>
        /// <param name="handler">Handler ID</param>
        /// <returns>Department instance</returns>
        public async Task<DepartmentVM> SaveDepartment(DepartmentVM departmentVM, int handler)
        {
            Department entity = mapper.Map<Department>(departmentVM);
            Department parent = null;
            if (!string.IsNullOrEmpty(departmentVM.Ma_DV_Cha))
            {
                parent = await this.departmentRepository.GetDepartmentByCode(departmentVM.Ma_DV_Cha);
            }
            
            if (parent != null)
            {
                entity.Id_DV_Cha = parent.Id_DonVi;
            }
            else
            {
                entity.Id_DV_Cha = 0;
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

            return this.mapper.Map<DepartmentVM>(entity);
        }

        /// <summary>
        /// update email
        /// </summary>
        /// <param name="emailDepartmentVM">the email of department vm</param>
        /// <param name="handler">the employee id current</param>
        /// <param name="departmentId">the department id current</param>
        /// <returns>the task</returns>
        public async Task<DepartmentVM> UpdateEmail(EmailDepartmentVM emailDepartmentVM, int handler, int departmentId)
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

            return this.mapper.Map<DepartmentVM>(department);
        }

        /// <summary>
        /// Gets child nodes by parent Id
        /// </summary>
        /// <param name="parentId">Parent Id</param>
        /// <param name="handler">Who is doing this action</param>
        /// <returns>List node</returns>
        public async Task<IEnumerable<DepartmentNodeVM>> GetChildNodes(int parentId, int handler)
        {
            return await this.departmentRepository.GetChildNodes(parentId, handler);
        }

        /// <summary>
        /// Gets a department by id
        /// </summary>
        /// <param name="departmentId">Department Id</param>
        /// <param name="handler">Who is doing this action</param>
        /// <returns>A DepartmentVM instance</returns>
        public async Task<DepartmentVM> GetById(int departmentId, int handler)
        {
            var department = await this.departmentRepository.GetDepartmentByIdAsync(departmentId, handler);
            if (department == null)
            {
                throw new Exception("Đơn vị này không tồn tại hoặc người dùng không có quyền truy cập");
            }

            return this.mapper.Map<DepartmentVM>(department);
        }

        /// <summary>
        /// Get departments with format select Item
        /// </summary>
        /// <param name="handler">Who is doing this action</param>
        public async Task<IEnumerable<DepartmentNodeVM>> GetSelectItems(int handler)
        {
            var departments = (await this.departmentRepository.GetAllDepartmentAccessed(handler)).ToList();
            var list = new List<DepartmentNodeVM>();
            var length = departments.Count();
            for (var i = 0; i < length; i++)
            {
                var level = 0;
                for (var j = i - 1; j >= 0; j--)
                {
                    if (list[j].Id_DonVi == departments[i].Id_DV_Cha)
                    {
                        level = list[j].Level;
                    }
                }

                list.Add(new DepartmentNodeVM
                {
                    Id_DonVi = departments[i].Id_DonVi,
                    Ten_DonVi = departments[i].Ten_DonVi,
                    Ma_DonVi = departments[i].Ma_DonVi,
                    Level = level + 1
                });
            }

            return list;
        }

        private async Task PublishUpdatingDepartment(string oldDepartmentCode, Department department)
        {
            var parent = await this.departmentRepository.GetByIdAsync(department.Id_DV_Cha);
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
            var parent = await this.departmentRepository.GetByIdAsync(department.Id_DV_Cha);
            var message = this.mapper.Map<DepartmentMessage>(department);
            if (parent != null)
            {
                message.Ma_DV_Cha = parent.Ma_DonVi;
            }

            rabbitMQService.Publish(message, RabbitActionTypes.Create, RabbitEntities.Department);
        }
    }
}
