using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebClient.Core.Entities;
using WebClient.Core.Responses;

namespace WebClient.Repositories.Interfaces
{
    public interface IAppRepository : IBaseRepository<App>
    {
        /// <summary>
        /// Get all app
        /// </summary>
        /// <returns>List app</returns>
        Task<IEnumerable<AppResponse>> GetAll();

        /// <summary>
        /// Get apps that the user can accessed
        /// </summary>
        /// <param name="handler">Who doing this action</param>
        /// <returns>List app</returns>
        Task<IEnumerable<AppResponse>> GetUserApps(int handler);

        /// <summary>
        /// Sets apps that the user can accessed
        /// </summary>
        /// <param name="id_NguoiDung">Id user</param>
        /// <param name="idApps">Id Apps</param>
        /// <param name="handler">Who doing this action</param>
        /// <returns>A void task</returns>
        Task SetPermission(int id_NguoiDung, string idApps, int handler);
    }
}
