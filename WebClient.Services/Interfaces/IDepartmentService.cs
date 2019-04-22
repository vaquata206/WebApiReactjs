using System.Collections.Generic;
using System.Threading.Tasks;
using WebClient.Core.Entities;
using WebClient.Core.ViewModels;

namespace WebClient.Services.Interfaces
{
    public interface IDepartmentService
    {
        /// <summary>
        /// Id of the Department that will be deleted
        /// </summary>
        /// <param name="departmentCode">code of Department</param>
        /// <param name="handler">Handler Id</param>
        /// <returns>Department instance</returns>
        Task<Department> DeleteDepartmentAsync(string departmentCode, int handler);

        /// <summary>
        /// Save department
        /// </summary>
        /// <param name="departmentVM">Department VM</param>
        /// <param name="handler">Who is doing this action</param>
        /// <returns>Department instance</returns>
        Task<DepartmentVM> SaveDepartment(DepartmentVM department, int handler);

        /// <summary>
        /// update email
        /// </summary>
        /// <param name="emailDepartmentVM">the email of department vm</param>
        /// <param name="employeeId">the employee id current</param>
        /// <param name="departmentId">the department id current</param>
        /// <returns>the task</returns>
        Task<DepartmentVM> UpdateEmail(EmailDepartmentVM emailDepartmentVM, int employeeId, int departmentId); 

        /// <summary>
        /// Gets child nodes by parent Id
        /// </summary>
        /// <param name="parentId">Parent Id</param>
        /// <param name="handler">Who is doing this action</param>
        /// <returns>List node</returns>
        Task<IEnumerable<DepartmentNodeVM>> GetDepartmentsByParent(int parentId, int handler);

        /// <summary>
        /// Gets a department by id
        /// </summary>
        /// <param name="departmentId">Department Id</param>
        /// <param name="handler">Who is doing this action</param>
        /// <returns>A DepartmentVM instance</returns>
        Task<DepartmentVM> GetById(int departmentId, int handler);
        
        /// <summary>
        /// Get departments with format select Item
        /// </summary>
        /// <param name="handler">Who is doing this action</param>
        Task<IEnumerable<DepartmentNodeVM>> GetSelectItems(int handler);

        /// <summary>
        /// Gets child nodes by parent id. Include: Departments/employees
        /// </summary>
        /// <param name="id">parent id</param>
        /// <param name="handler">Who doing this action</param>
        /// <returns>Tree nodes</returns>
        Task<IEnumerable<TreeNode>> GetChildNodes(int id, int handler);
    }
}
