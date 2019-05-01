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
        /// Get list permission
        /// </summary>
        /// <returns>List permission</returns>
        Task<IEnumerable<Permission>> GetPermissions();
        
        /// <summary>
        /// Delete permission
        /// </summary>
        /// <param name="permissionId">Permisison id</param>
        /// <returns>A Task</returns>
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
