using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebClient.Core.Entities;

namespace WebClient.Repositories.Interfaces
{
    public interface IEmployeeRepository : IBaseRepository<Employee>
    {
        /// <summary>
        /// Update information's Employee
        /// </summary>
        /// <param name="employee">the employee</param>
        /// <returns>the employee after updated</returns>
        Task<Employee> UpdateInformationEmployee(Employee employee, int currUser);

        /// <summary>
        /// Get employee by idNhanVien
        /// </summary>
        /// <param name="idNhanVien"></param>
        /// <returns>the employee</returns>
        Task<Employee> GetEmployeeById(int idNhanVien);

        /// <summary>
        /// Get employees by department id
        /// </summary>
        /// <param name="deparmentId">Id of deparment</param>
        /// <returns>List employee</returns>
        Task<IEnumerable<Employee>> GetEmployeesByDeparmentId(int deparmentId);

        /// <summary>
        /// get all employees
        /// </summary>
        /// <returns>lst employees</returns>
        Task<IEnumerable<Employee>> GetAllEmployess();

        /// <summary>
        /// delete employee
        /// </summary>
        /// <returns>the task</returns>
        Task DeteteEmployee(Employee employee);

        /// <summary>
        /// insert new employee
        /// </summary>
        /// <param name="employee">the new employee</param>
        /// <param name="curUser">the curr idnhanvien</param>
        /// <returns>the task</returns>
        Task InsertEmployee(Employee employee, int curUser);
    }
}
