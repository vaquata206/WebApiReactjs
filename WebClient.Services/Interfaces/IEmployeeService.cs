using System.Collections.Generic;
using System.Threading.Tasks;
using WebClient.Core.Entities;
using WebClient.Core.ViewModels;

namespace WebClient.Services.Interfaces
{
    public interface IEmployeeService
    {
        /// <summary>
        /// get employee view model by id nhanvien
        /// </summary>
        /// <param name="idNhanVien">idnhanvien</param>
        /// <returns>the employee view model</returns>
        Task<EmployeeVM> GetEmployVMById(int idNhanVien);

        /// <summary>
        /// get all employees
        /// </summary>
        /// <returns>lst employees</returns>
        Task<IEnumerable<Employee>> GetAllEmployees();

        /// <summary>
        /// Update information employee
        /// </summary>
        /// <param name="employViewModel">the employee VM</param>
        /// <param name="userId">User id</param>
        /// <returns>the employeeVM after updated</returns>
        Task<Employee> UpdateInformationEmployee(EmployeeVM employViewModel, int userId);

        /// <summary>
        /// Get a employee by its id
        /// </summary>
        /// <param name="employeeId">employee id</param>
        /// <returns>The employee instance</returns>
        Task<Employee> GetEmployeeAsync(int employeeId);

        /// Get employees by department id
        /// </summary>
        /// <param name="deparmentId">Id of deparment</param>
        /// <returns>List employee</returns>
        Task<IEnumerable<Employee>> GetEmployeesByDeparmentId(int deparmentId);

        /// <summary>
        /// delete employee by id
        /// </summary>
        /// <param name="userCode">User code</param>
        /// <param name="currUserId">Current user id</param>
        /// <returns>the Task</returns>
        Task<Employee> DeleteEmployeeByCode(string userCode, int currUserId);

        /// <summary>
        /// save employee (insert or update)
        /// </summary>
        /// <param name="employeeVM">the employee VM</param>
        /// <param name="curUser">the current idNhanvien</param>
        /// <returns>the task</returns>
        Task<Employee> SaveEmployee(EmployeeVM employeeVM,int curUser);
    }
}
