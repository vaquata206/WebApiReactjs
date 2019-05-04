using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using WebClient.Core.Entities;
using WebClient.Core.Requests;
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

        /// <summary>
        /// Account repository
        /// </summary>
        private IAccountRepository accountRepository;

        /// <summary>
        /// Mapper
        /// </summary>
        private IMapper mapper;

        /// <summary>
        /// A constructor
        /// </summary>
        /// <param name="appRepository"></param>
        public AppService(IAppRepository appRepository, IAccountRepository accountRepository, IMapper mapper)
        {
            this.appRepository = appRepository;
            this.accountRepository = accountRepository;
            this.mapper = mapper;
        }

        /// <summary>
        /// Get a app by app id
        /// </summary>
        /// <param name="id">App id</param>
        /// <returns>App response</returns>
        public async Task<AppResponse> Get(int id){
            var app = await this.appRepository.GetByIdAsync(id);
            if (app == null)
            {
                throw new Exception("Chương trình không tồn tại");
            }

            return this.mapper.Map<AppResponse>(app);
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
        /// Save a app
        /// </summary>
        /// <param name="appRequest">App request</param>
        /// <param name="handler">Who doing this action</param>
        /// <returns>A task</returns>
        public async Task Save(AppRequest appRequest, int handler)
        {
            App app = this.mapper.Map<App>(appRequest);
            if (appRequest.Id_ChuongTrinh == 0)
            {
                app.Ma_ChuongTrinh = Guid.NewGuid().ToString();
                app.Tinh_Trang = 1;
                await this.appRepository.AddAsync(app);
            }
            else
            {
                var old = await this.appRepository.GetByIdAsync(appRequest.Id_ChuongTrinh);
                if (old == null)
                {
                    throw new Exception("Chương trình không tồn tại");
                }

                app.Id_ChuongTrinh = old.Id_ChuongTrinh;
                app.Ma_ChuongTrinh = old.Ma_ChuongTrinh;
                app.Tinh_Trang = 1;

                await this.appRepository.UpdateAsync(app);
            }
        }

        /// <summary>
        /// Delete a app
        /// </summary>
        /// <param name="id">App id</param>
        /// <returns>A task</returns>
        public async Task Delete(int id)
        {
            var app = await this.appRepository.GetByIdAsync(id);
            if (app == null)
            {
                throw new Exception("Chương trình không tồn tại");
            }

            app.Tinh_Trang = 0;
            await this.appRepository.UpdateAsync(app);
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
