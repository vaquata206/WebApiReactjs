using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebClient.Core.Entities;
using WebClient.Core.ViewModels;

namespace WebClient.Services.Interfaces
{
    public interface IDepartmentService
    {
        /// <summary>
        /// Get tree nodes that are children of the department
        /// </summary>
        /// <param name="departmentId">Id of department</param>
        /// <returns>Tree nodes</returns>
        Task<IEnumerable<TreeNode>> GetTreeNodeChildrenOfDepartment(int departmentId, int accountId);

        /// <summary>
        /// Get department by id
        /// </summary
        /// <param name="id">The id of department</param>
        /// <returns>The department</returns>
        Task<Department> GetDepartmentById(int id);

        /// Get all Departments
        /// </summary>
        /// <returns>List Departments</returns>
        Task<IEnumerable<Department>> GetAllAsync();

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
        /// <param name="handler">Handler ID</param>
        /// <returns>Department instance</returns>
        Task<Department> SaveDepartment(DepartmentVM department, int handler);

        /// <summary>
        /// get department VM by id donvi
        /// </summary>
        /// <returns></returns>
        Task<DepartmentVM> GetDeparmentVMByID(int idDonVi);

        /// <summary>
        /// convert jstree
        /// </summary>
        /// <param name="list">the list departments</param>
        /// <returns>list departments converted</returns>
        IEnumerable<Department> ConvertToTree(IEnumerable<Department> list, int idRoot = 0);

        /// <summary>
        /// Get departments with condition: to idDepartmentStart from idDepartmentEnd.
        /// </summary>
        /// <param name="idDepartment">id department start</param>
        /// <param name="accountId">Account Id whose user is performing this action</param>
        /// <param name="idDepartmentEnd">id department End. Default = -1, if idDepartmentEnd = -1 then return all.</param>
        /// <returns>list departments </returns>
        Task<IEnumerable<Department>> GetTreeDepartmentsdWithTerm(int accountId, int idDepartmentStart, int idDepartmentEnd = -1);

        /// <summary>
        /// update email
        /// </summary>
        /// <param name="emailDepartmentVM">the email of department vm</param>
        /// <param name="employeeId">the employee id current</param>
        /// <param name="departmentId">the department id current</param>
        /// <returns>the task</returns>
        Task<Department> UpdateEmail(EmailDepartmentVM emailDepartmentVM, int employeeId, int departmentId); 

        /// <summary>
        /// Gets children that are controlled by the account by parent Id
        /// </summary>
        /// <param name="parentId">parent department id</param>
        /// <param name="accountId">Account id</param>
        /// <returns>List department</returns>
        Task<IEnumerable<Department>> GetChildrenByParentId(int parentId, int accountId);

        /// <summary>
        /// Gets all children by parent Id
        /// </summary>
        /// <param name="parentId">Parent department id</param>
        /// <returns>List department</returns>
        Task<IEnumerable<Department>> GetChildrenByParentId(int parentId);

        /// <summary>
        /// Gets id departments that managed by the account
        /// </summary>
        /// <param name="accountId">Account id</param>
        /// <returns>List id</returns>
        Task<IEnumerable<int>> GetControlledDepartmentIds(int accountId);
    }
}
