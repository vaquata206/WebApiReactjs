using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebClient.Core.Request;
using WebClient.Core.Responses;
using WebClient.Repositories.Interfaces;
using WebClient.Services.Interfaces;

namespace WebClient.Services.Implements
{
    public class AppService : IAppService
    {
        /// <summary>
        /// App repository
        /// </summary>
        private IAppRepository appRepository;

        private IAccountRepository accountRepository;

        /// <summary>
        /// A constructor
        /// </summary>
        /// <param name="appRepository"></param>
        public AppService(IAppRepository appRepository, IAccountRepository accountRepository)
        {
            this.appRepository = appRepository;
            this.accountRepository = accountRepository;
        }

        /// <summary>
        /// Get all app
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<AppResponse>> GetAll()
        {
            return await this.appRepository.GetAll();
        }

        /// <summary>
        /// Get apps that the user can accessed
        /// </summary>
        /// <param name="handler">Who doing this action</param>
        /// <returns>List app</returns>
        public async Task<IEnumerable<AppResponse>> GetUserAppsAsync(int handler)
        {
            return await this.appRepository.GetUserApps(handler);
        }

        /// <summary>
        /// Sets apps that the user can accessed
        /// </summary>
        /// <param name="userApps">User App Request</param>
        /// <param name="handler">Who doing this action</param>
        /// <returns>A void task</returns>
        public async Task SetPermission(UserAppsRequest userApps, int handler)
        {
            var account = await this.accountRepository.GetByIdAsync(userApps.Id_NguoiDung);
            if (account.Quan_Tri == 1)
            {
                throw new Exception("Không thể cập nhật đối với tài khoản này");
            }

            var strIds = userApps.Id_ChuongTrinhs == null ? "" : string.Join(",", userApps.Id_ChuongTrinhs);

            await this.appRepository.SetPermission(
                userApps.Id_NguoiDung,
                strIds, 
                handler);
        }
    }
}
