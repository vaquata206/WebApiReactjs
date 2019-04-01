using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebClient.Core.Entities;

namespace WebClient.Repositories.Interfaces
{
    public interface IPermissionRepository : IBaseRepository<Permission>
    {
        /// <summary>
        /// Add a permission
        /// </summary>
        /// <param name="permission">The permission</param>
        /// <returns>A task</returns>
        Task AddPermissionAsync(Permission permission);

        /// <summary>
        /// Update a permission
        /// </summary>
        /// <param name="permission">The permission</param>
        /// <returns>A task</returns>
        Task UpdatePermissionAsync(Permission permission);

        /// <summary>
        /// Get permission by id 
        /// </summary>
        /// <param name="permissionId">permission id</param>
        /// <returns>The permission</returns>
        Task<Permission> GetPermissionByIdAsync(int permissionId);

        /// <summary>
        /// Get list permission
        /// </summary>
        /// <returns>List permission</returns>
        Task<IEnumerable<Permission>> GetPermissions();

        Task DeleteAsync(int permissionId);

        /// <summary>
        /// Sets departments that the account is avaiabled work
        /// </summary>
        /// <param name="accountId">Account id</param>
        /// <param name="departmentIds">Department ids</param>
        /// <param name="handlerId">Id of user who is performing this action</param>
        Task SetDepartmentsAsync(int accountId, int[] departmentIds, int handlerId);
    }
}
