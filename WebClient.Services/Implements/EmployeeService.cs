using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebClient.Core.Entities;
using WebClient.Core.ViewModels;
using WebClient.Repositories.Interfaces;
using WebClient.Services.Interfaces;

namespace WebClient.Services.Implements
{
    public class EmployeeService : IEmployeeService
    {
        /// <summary>
        /// employe service interface
        /// </summary>
        private IEmployeeRepository _employee;
        /// <summary>
        /// Create a field to store the mapper object
        /// </summary>
        private readonly IMapper _mapper;
        
        public EmployeeService(IEmployeeRepository employee, IMapper mapper)
        {
            this._employee = employee;
            this._mapper = mapper;
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
                employeVM = this._mapper.Map<EmployeeVM>(employee);
            }
            return employeVM;
        }

        /// <summary>
        /// Update information employee
        /// </summary>
        /// <param name="employViewModel">Employee viewmodal</param>
        /// <param name="userId">User Id</param>
        /// <returns>Employee Viewmodal</returns>
        public async Task<EmployeeVM> UpdateInformationEmployee(EmployeeVM employViewModel, int userId)
        {
            Employee emp = this._mapper.Map<Employee>(employViewModel);
            emp = await this._employee.UpdateInformationEmployee(emp, userId);
            employViewModel = this._mapper.Map<EmployeeVM>(emp);
            return employViewModel;
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
        /// Delete employee bi id
        /// </summary>
        /// <param name="idNhanVien">idNhanVien</param>
        /// <param name="currUserId">Current user id</param>
        /// <returns>the task</returns>
        public async Task DeleteEmployeeById(int idNhanVien, int currUserId)
        {
            var employee = await this._employee.GetEmployeeById(idNhanVien);
            if (employee == null)
            {
                throw new Exception("Nhân viên này không tồn tại");
            }

            employee.Id_NV_CapNhat = currUserId;
            employee.Ngay_CapNhat = DateTime.Now;

            await this._employee.DeteteEmployee(employee);
        }

        /// <summary>
        /// save employee (insert or update)
        /// </summary>
        /// <param name="employeeVM">the employee VM</param>
        /// <param name="curUser">the current idNhanvien</param>
        /// <returns>the task</returns>
        public async Task SaveEmployee(EmployeeVM employeeVM,int curUser)
        {
            Employee emp = null;
            
            if (employeeVM.MaNhanVien != null)
            {
                emp = _mapper.Map<Employee>(employeeVM);
                await this.ValidationEmployee(emp);
                await this._employee.UpdateInformationEmployee(emp,curUser);
            }
            else
            {
                employeeVM.MaNhanVien = "NV" + DateTime.Now.Ticks;
                emp = _mapper.Map<Employee>(employeeVM);
                await this.ValidationEmployee(emp);
                await this._employee.InsertEmployee(emp,curUser);
            }
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
            employees = employees.Where(x => x.Id_NhanVien != emp.Id_NhanVien).ToList();
            var valSoCmnd = employees.Where(x => x.So_CMND.Equals(emp.So_CMND));
            if(valSoCmnd.Count() > 0)
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
    }
}
