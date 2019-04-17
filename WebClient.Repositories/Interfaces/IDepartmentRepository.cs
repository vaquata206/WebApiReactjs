using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebClient.Core.Entities;
using WebClient.Core.ViewModels;

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
        /// Gets all children by parent Id
        /// </summary>
        /// <param name="parentId">Parent department id</param>
        /// <returns>List department</returns>
        Task<IEnumerable<Department>> GetDepartmentsByIdParent(int parentId);

        /// <summary>
        /// Gets department that accessed by the handler with department id
        /// </summary>
        /// <param name="idDonVi">department id</param>
        /// <param name="handler">who is doing this action</param>
        /// <returns></returns>
        Task<Department> GetDepartmentByIdAsync(int idDonVi, int handler);

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
        /// Update email
        /// </summary>
        /// <param name="department">Department instance</param>
        /// <returns>Department instance</returns>
        Task<Department> UpdateEmail(Department department);

        /// <summary>
        /// Gets child nodes by parent Id
        /// </summary>
        /// <param name="parentId">Parent Id</param>
        /// <param name="handler">Who is doing this action</param>
        /// <returns>List node</returns>
        Task<IEnumerable<DepartmentNodeVM>> GetChildNodes(int parentId, int handler);

        /// <summary>
        /// Get all department that the user is accessed
        /// </summary>
        /// <param name="handler">Who is doing this action</param>
        /// <returns>List department</returns>
        Task<IEnumerable<Department>> GetAllDepartmentAccessed(int handler);
    }
}