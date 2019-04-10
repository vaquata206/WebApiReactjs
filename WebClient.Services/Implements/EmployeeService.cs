using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using WebClient.Core.Entities;
using WebClient.Core.Messages;
using WebClient.Core.ViewModels;
using WebClient.Repositories.Interfaces;
using WebClient.Services.Interfaces;

namespace WebClient.Services.Implements
{
    public class EmployeeService : IEmployeeService
    {
        /// <summary>
        /// RabbitMQ service
        /// </summary>
        private IRabbitMQService rabbitMQService;
        
        /// <summary>
        /// employe service interface
        /// </summary>
        private IEmployeeRepository _employee;

        /// <summary>
        /// Department repository
        /// </summary>
        private IDepartmentRepository departmentRepository;
        
        /// <summary>
        /// Account repository
        /// </summary>
        private IAccountRepository accountRepository;
        
        /// <summary>
        /// Create a field to store the mapper object
        /// </summary>
        private readonly IMapper mapper;
        
        public EmployeeService(IEmployeeRepository employee, IAccountRepository accountRepository, IDepartmentRepository departmentRepository,IRabbitMQService rabbitMQService, IMapper mapper)
        {
            this._employee = employee;
            this.departmentRepository = departmentRepository;
            this.accountRepository = accountRepository;
            this.rabbitMQService = rabbitMQService;
            this.mapper = mapper;
        }

        /// <summary>
        /// get employee view model by id nhanvien
        /// </summary>
        /// <param name="idNhanVien">idnhanvien</param>
        /// <returns>the employee view model</returns>
        public async Task<EmployeeVM> GetEmployVMById(int idNhanVien)
        {
            var employee = await this._employee.GetEmployeeById(idNhanVien);
            EmployeeVM employeVM = null;
            if (employee != null)
            {
                employeVM = this.mapper.Map<EmployeeVM>(employee);
            }
            return employeVM;
        }

        /// <summary>
        /// Update information employee
        /// </summary>
        /// <param name="employeeVM">Employee viewmodal</param>
        /// <param name="userId">User Id</param>
        /// <returns>Employee Viewmodal</returns>
        public async Task<Employee> UpdateInformationEmployee(EmployeeVM employeeVM, int userId)
        {
            var employee = await this._employee.GetEmployeeByCode(employeeVM.MaNhanVien);

            if (employee == null)
            {
                throw new Exception("Nhân viên không tồn tại");
            }

            var newEmployee = this.mapper.Map<Employee>(employeeVM);

            newEmployee.Id_NhanVien = employee.Id_NhanVien;

            newEmployee = await this._employee.UpdateInformationEmployee(newEmployee, userId);

            await this.PublishUpdatingEmployee(employee.Ma_NhanVien, newEmployee);

            return employee;
        }

        /// <summary>
        /// Get a employee by its id
        /// </summary>
        /// <param name="employeeId">employee id</param>
        /// <returns>The employee instance</returns>
        public async Task<Employee> GetEmployeeAsync(int employeeId)
        {
            return await this._employee.GetEmployeeById(employeeId);
        }

        /// get all employees
        /// </summary>
        /// <returns>lst employees</returns>
        public async Task<IEnumerable<Employee>> GetAllEmployees()
        {
            return await _employee.GetAllEmployess();
        }

        /// <summary>
        /// Get employees by department id
        /// </summary>
        /// <param name="deparmentId">Id of deparment</param>
        /// <returns>List employee</returns>
        public async Task<IEnumerable<Employee>> GetEmployeesByDeparmentId(int deparmentId)
        {
            return await _employee.GetEmployeesByDeparmentId(deparmentId);
        }

        /// <summary>
        /// Delete employee by employee code
        /// </summary>
        /// <param name="employeeCode">idNhanVien</param>
        /// <param name="currUserId">Current user id</param>
        /// <returns>the task</returns>
        public async Task<Employee> DeleteEmployeeByCode(string employeeCode, int currUserId)
        {
            var employee = await this._employee.GetEmployeeByCode(employeeCode);
            if (employee == null)
            {
                throw new Exception("Nhân viên này không tồn tại");
            }

            employee = await this._employee.DeteteEmployee(employee, currUserId);

            await this.PublishUpdatingEmployee(employee.Ma_NhanVien, employee);

            var accounts = await this.accountRepository.GetAllAccountsByEmployeeId(employee.Id_NhanVien);
            foreach(var a in accounts)
            {
                var message = this.mapper.Map<AccountMessage>(a);
                message.Ma_NhanVien = employee.Ma_NhanVien;
                message.Ma_NguoiDung_Cu = a.Ma_NguoiDung;
                // update accounts that are deleted when the employee is deleted
                rabbitMQService.Publish(message, RabbitActionTypes.Update, RabbitEntities.Account);
            }

            return employee;
        }

        /// <summary>
        /// save employee (insert or update)
        /// </summary>
        /// <param name="employeeVM">the employee VM</param>
        /// <param name="curUser">the current idNhanvien</param>
        /// <returns>the task</returns>
        public async Task<Employee> SaveEmployee(EmployeeVM employeeVM, int curUser)
        {
            var department = await this.departmentRepository.GetDepartmentByCode(employeeVM.Ma_DonVi);

            if (department == null)
            {
                throw new Exception("Đơn vị không tồn tại");
            }

            Employee emp = mapper.Map<Employee>(employeeVM);
            await this.ValidationEmployee(emp);

            emp.Id_DonVi = department.Id_DonVi;
            if (employeeVM.MaNhanVien != null)
            {
                var oldEmployee = await this._employee.GetEmployeeByCode(employeeVM.MaNhanVien);
                if (oldEmployee == null)
                {
                    throw new Exception("Nhân viên không tồn tại");
                }

                emp.Id_NhanVien = oldEmployee.Id_NhanVien;

                emp = await this._employee.UpdateInformationEmployee(emp, curUser);
                await PublishUpdatingEmployee(employeeVM.MaNhanVien, emp);
            }
            else
            {
                emp.Ma_NhanVien = "NV" + DateTime.Now.Ticks;
                emp = await this._employee.InsertEmployee(emp,curUser);
                await PublishCreatingEmployee(emp);
            }

            return emp;
        }

        /// <summary>
        /// validation employ with: so cmnd, nam_sinh, ngay tao cmnd.
        /// </summary>
        /// <param name="emp">the employee</param>
        /// <returns>the exception</returns>
        private async Task ValidationEmployee(Employee emp)
        {
            IEnumerable<Employee> employees;
            employees = await this._employee.GetAllEmployess();
            var valSoCmnd = employees.Where(x => x.Ma_NhanVien != emp.Ma_NhanVien && x.So_CMND.Equals(emp.So_CMND)).Count();
            if(valSoCmnd > 0)
            {
                throw new Exception("Số CMND đã tồn tại!!");
            }
            if(emp.Nam_Sinh >= DateTime.Now)
            {
                throw new Exception("Ngày sinh không hợp lệ!!");
            }
            if(emp.NgayCap_CMND >= DateTime.Now)
            {
                throw new Exception("Ngày cấp CMND không hợp lệ!!");
            }
        }

        /// <summary>
        /// Publish updating employee
        /// </summary>
        /// <param name="employee">The employee</param>
        private async Task PublishUpdatingEmployee(string oldEmployee, Employee newEmployee)
        {
            var message = this.mapper.Map<EmployeeMessage>(newEmployee);
            var department = await this.departmentRepository.GetByIdAsync(newEmployee.Id_DonVi);
            message.Ma_NhanVien_Cu = oldEmployee;
            message.Ma_DonVi = department.Ma_DonVi;

            rabbitMQService.Publish(
                content: message,
                action: RabbitActionTypes.Update,
                name: RabbitEntities.Employee
                );
        }

        /// <summary>
        /// publish creating employee
        /// </summary>
        /// <param name="employee">The employee</param>
        /// <returns>A void task</returns>
        private async Task PublishCreatingEmployee(Employee employee)
        {
            var message = this.mapper.Map<EmployeeMessage>(employee);
            var department = await this.departmentRepository.GetByIdAsync(employee.Id_DonVi);
            message.Ma_DonVi = department.Ma_DonVi;
            rabbitMQService.Publish(
                content: message,
                action: RabbitActionTypes.Create,
                name: RabbitEntities.Employee
                );
        }
    }
}
