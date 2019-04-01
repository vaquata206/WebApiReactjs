﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebClient.Core.Entities;
using WebClient.Core.Helper;
using WebClient.Core.ViewModels;
using WebClient.Repositories.Interfaces;
using WebClient.Services.Interfaces;

namespace WebClient.Services.Implements
{
    public class PermissionService : IPermissionService
    {
        private IPermissionRepository permissionRepository;
        private IAccountRepository accountRepository;
        public PermissionService(IPermissionRepository permissionRepository, IAccountRepository accountRepository)
        {
            this.permissionRepository = permissionRepository;
            this.accountRepository = accountRepository;
        }

        /// <summary>
        /// Add a permission
        /// </summary>
        /// <param name="permissionVM">The permission</param>
        /// <returns>A Permission</returns>
        public async Task<Permission> SavePermissionAsync(PermissionVM permissionVM)
        {
            Permission permission;
            if (permissionVM.Id_Quyen.HasValue && permissionVM.Id_Quyen.Value != 0)
            {
                // edit
                permission = await this.permissionRepository.GetPermissionByIdAsync(permissionVM.Id_Quyen.Value);
                if (permission == null)
                {
                    throw new Exception("Quyền này không tồn tại");
                }

                permission.Ten_Quyen = permissionVM.Ten_Quyen;
                permission.Ghi_Chu = permissionVM.Ghi_Chu;

                await this.permissionRepository.UpdatePermissionAsync(permission);
            }
            else
            {
                // add
                permission = new Permission
                {
                    Ma_Quyen = "MQ" + string.Format("{0:yyyyMMddhhmmss}", DateTime.Now),
                    Ghi_Chu = permissionVM.Ghi_Chu,
                    Ten_Quyen = permissionVM.Ten_Quyen,
                    Tinh_Trang = true
                };

                await this.permissionRepository.AddPermissionAsync(permission);
            }

            return permission;
        }

        /// <summary>
        /// Get the permission by id
        /// </summary>
        /// <param name="permissionId">Id of permission</param>
        /// <returns>A Permission</returns>
        public async Task<Permission> GetPermissionByIdAsync(int permissionId)
        {
            return await this.permissionRepository.GetPermissionByIdAsync(permissionId);
        }

        /// <summary>
        /// Get list permission
        /// </summary>
        /// <returns>list permission</returns>
        public async Task<IEnumerable<Permission>> GetPermissions()
        {
            return await this.permissionRepository.GetPermissions();
        }

        /// <summary>
        /// Delete a permission
        /// </summary>
        /// <param name="permissionId">Id of permission</param>
        /// <returns>A task</returns>
        public async Task DeleteAsync(int permissionId)
        {
            await this.permissionRepository.DeleteAsync(permissionId);
        }

        /// <summary>
        /// Sets departments that the account is avaiabled work
        /// </summary>
        /// <param name="accountId">Account id</param>
        /// <param name="departmentIds">Department ids</param>
        /// <param name="handlerId">Id of user who is performing this action</param>
        public async Task SetDepartmentsAsync(int accountId, int[] departmentIds, int handlerId)
        {
            var account = await this.accountRepository.GetByIdAsync(accountId);
            if (account.Quan_Tri == Constants.States.Actived)
            {
                throw new Exception("Không thể thay đổi với tài khoản này");
            }

            await this.permissionRepository.SetDepartmentsAsync(accountId, departmentIds, handlerId);
        }
    }
}