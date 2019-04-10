using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebClient.Core.Entities;

namespace WebClient.Repositories.Interfaces
{
    public interface IDepartmentRepository : IBaseRepository<Department>
    {
        /// <summary>
        /// Get department by department code
        /// </summary>
        /// <param name="departmentCode">Department code</param>
        /// <returns>A department instance</returns>
        Task<Department> GetDepartmentByCode(string departmentCode);

        /// <summary>
        /// Gets children that are controlled by the account by parent Id
        /// </summary>
        /// <param name="parentId">parent department id</param>
        /// <param name="accountId">Account id</param>
        /// <returns>List department</returns>
        Task<IEnumerable<Department>> GetDepartmentsByIdParent(int idParent, int accountId);

        /// <summary>
        /// Gets all children by parent Id
        /// </summary>
        /// <param name="parentId">Parent department id</param>
        /// <returns>List department</returns>
        Task<IEnumerable<Department>> GetDepartmentsByIdParent(int parentId);

        /// <summary>
        /// Get department by employee id
        /// </summary>
        /// <param name="employeeId">employee id</param>
        /// <returns>The department</returns>
        Task<Department> GetDepartmentByEmployeeId(int employeeId);

        /// Get all units
        /// </summary>
        /// <returns>List units</returns>
        Task<IEnumerable<Department>> GetAllAsync();

        /// <summary>
        /// Gets a Department by id
        /// </summary>
        /// <param name="idonvi">Id of Department</param>
        /// <returns>A Department</returns>
        Task<Department> GetDepartmentByIdAsync(int idDonVi);

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
        Task<int> DeleteDepartmentAsync(int idDonVi, int handler);

        /// <summary>
        /// Updates a Department
        /// </summary>
        /// <param name="Department">The Department</param>
        /// <returns>the task</returns>
        Task<Department> UpdateDepartmentAsync(Department department);

        /// <summary>
        /// Insert new Department
        /// </summary>
        /// <param name="department">the new Department</param>
        /// <returns>the task</returns>
        Task<Department> AddDepartmentAsync(Department department);

        /// <summary>
        /// Get departments with condition: to idDepartmentStart from idDepartmentEnd.
        /// </summary>
        /// <param name="idDepartment">id department start</param>
        /// <param name="accountId">Account Id whose user is performing this action</param>
        /// <param name="idDepartmentEnd">id department End, if idDepartmentEnd = -1 then return all.</param>
        /// <returns>list departments </returns>
        Task<IEnumerable<Department>> GetDepartmentsWithTerm(int accountId, int idDepartment, int idDepartmentEnd = -1);

        /// <summary>
        /// Gets id departments that managed by the account
        /// </summary>
        /// <param name="accountId">Account id</param>
        /// <returns>List department</returns>
        Task<IEnumerable<Department>> GetControlledDepartments(int accountId);

        Task<Department> UpdateEmail(Department department);
    }
}