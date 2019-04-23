using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebClient.Core.Requests;
using WebClient.Core.Responses;

namespace WebClient.Services.Interfaces
{
    public interface IAppService
    {
        /// <summary>
        /// Get a app by app id
        /// </summary>
        /// <param name="id">App id</param>
        /// <returns>App response</returns>
        Task<AppResponse> Get(int id);

        /// <summary>
        /// Get all app
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<AppResponse>> GetAll();

        /// <summary>
        /// Get apps that the user can accessed
        /// </summary>
        /// <param name="handler">Who doing this action</param>
        /// <returns>List app</returns>
        Task<IEnumerable<AppResponse>> GetUserAppsAsync(int handler);

        /// <summary>
        /// Sets apps that the user can accessed
        /// </summary>
        /// <param name="userApps">User App Request</param>
        /// <param name="handler">Who doing this action</param>
        /// <returns>A void task</returns>
        Task SetPermission(UserAppsRequest userApps, int handler);

        /// <summary>
        /// Save a app
        /// </summary>
        /// <param name="appRequest">App request</param>
        /// <param name="handler">Who doing this action</param>
        /// <returns>A task</returns>
        Task Save(AppRequest appRequest, int handler);

        /// <summary>
        /// Delete a app
        /// </summary>
        /// <param name="id">App id</param>
        /// <returns>A task</returns>
        Task Delete(int id);
    }
}
