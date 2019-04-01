using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebClient.Core.Helper;
using WebClient.Repositories.Interfaces;
using WebClient.Services.Interfaces;

namespace WebClient.Services.Implements
{
    public class EmployeePermissionService : IEmployeePermissionService
    {
        private IAccountRepository accountRepository;
        private IEmployeePermissionRepository employeePermissionRepository;
        public EmployeePermissionService(IEmployeePermissionRepository employeePermissionRepository, IAccountRepository accountRepository)
        {
            this.employeePermissionRepository = employeePermissionRepository;
            this.accountRepository = accountRepository;
        }

        public async Task SetPermissionsForEmployee(IEnumerable<int> ids, int userId, int handler)
        {
            var account = await this.accountRepository.GetByIdAsync(userId);
            if (account == null)
            {
                throw new Exception("Tài khoản này không tồn tại");
            }
            else if (account.Quan_Tri == Constants.States.Actived)
            {
                throw new Exception("Không thể cấp quyền với tài khoản này");
            }

            await this.employeePermissionRepository.SetPermissionsForUser(ids, userId, handler);
        }

        /// <summary>
        /// Get id of permissions that a user can accepts
        /// </summary>
        /// <param name="userId">user id</param>
        /// <returns>permission ids</returns>
        public async Task<IEnumerable<int>> GetIdPermissionsOfUser(int userId)
        {
            return await this.employeePermissionRepository.GetIdPermissionsOfUser(userId);
        }

        public async Task SetFeaturesForEmployee(IEnumerable<int> ids, int idEmployee, int idUser)
        {
            await this.employeePermissionRepository.SetFeaturesForEmployee(ids, idEmployee, idUser);
        }
    }
}
