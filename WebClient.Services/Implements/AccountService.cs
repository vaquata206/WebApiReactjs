using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebClient.Core.Entities;
using WebClient.Core.Helper;
using WebClient.Core.ViewModels;
using WebClient.Repositories.Interfaces;
using WebClient.Services.Interfaces;

namespace WebClient.Services.Implements
{
    /// <summary>
    /// Account service
    /// </summary>
    public class AccountService : IAccountService
    {
        /// <summary>
        /// account repository
        /// </summary>
        private IAccountRepository account;

        /// <summary>
        /// department repository
        /// </summary>
        private IDepartmentRepository departmentRepository;

        /// <summary>
        /// A constructor
        /// </summary>
        /// <param name="account">account repository</param>
        /// <param name="departmentRepository">department repository</param>
        public AccountService(IAccountRepository account, IDepartmentRepository departmentRepository)
        {
            this.account = account;
            this.departmentRepository = departmentRepository;
        }

        /// <summary>
        /// Login the user
        /// </summary>
        /// <param name="username">The username of account</param>
        /// <param name="password">The user's password</param>
        public async Task<AccountInfo> LoginAsync(string username, string password)
        {
            var account =  await this.account.LoginAsync(username, password);
            return account;
        }

        /// <summary>
        /// update information account
        /// </summary>
        /// <param name="account">account with new information</param>
        void IAccountService.UpdateInformationAccount(Account acc)
        {
            this.account.UpdateInformationAccount(acc);
        }

        public async Task<Account> GetAccountByUsername(string username)
        {
            return await this.account.GetAccountByUsername(username);
        }

        public async Task<bool> ChangePassword(string username, int idNhanvien, string matKhauCu, string matKhauMoi)
        {
            return await this.account.ChangePassword(username, idNhanvien, matKhauCu, matKhauMoi);
        }

        /// <summary>
        /// Gets employee's accounts
        /// </summary>
        /// <param name="employeeId">Employee id</param>
        /// <returns>List account</returns>
        public async Task<IEnumerable<Account>> GetAccountsByEmployeeId(int employeeId)
        {
            return await this.account.GetAccountsByEmployeeId(employeeId);
        }

        /// <summary>
        /// Create a new account for a employee
        /// </summary>
        /// <param name="accountVM">Account VM</param>
        /// <param name="currentUserId">Current user id</param>
        /// <returns>A void task</returns>
        public async Task CreateAccount(AccountVM accountVM, int currentUserId)
        {
            var account = new Account
            {
                Id_NhanVien = accountVM.Id_NhanVien,
                UserName = accountVM.UserName,
                MatKhau = Common.MD5Hash(accountVM.MatKhau),
                Id_NV_KhoiTao = currentUserId,
                Ngay_KhoiTao = DateTime.Now,
                Ma_NguoiDung = "user" + DateTime.Now.ToString("yyyyMMddHHmmss"),
                Tinh_Trang = 1,
                Quan_Tri = 0,
                Id_VaiTro = 3,
                Ngay_CapNhat = DateTime.Now
            };

            await this.account.AddAsync(account);
        }

        /// <summary>
        /// Delete a account
        /// </summary>
        /// <param name="accountId">Account id</param>
        /// <param name="currUserId">Current user id</param>
        /// <returns>A account</returns>
        public async Task<Account> DeleteAccount(int accountId, int currUserId)
        {
            var account = await this.account.GetByIdAsync(accountId, true);
            if (account == null)
            {
                throw new Exception("Tài khoản này không tồn tại");
            }

            if (account.Quan_Tri == 1)
            {
                throw new Exception("Tài khoản này không được xóa");
            }

            account.Tinh_Trang = 0;
            account.Ngay_CapNhat = DateTime.Now;

            await this.account.UpdateAsync(account);

            return account;
        }

        /// <summary>
        /// Reset password of a account
        /// </summary>
        /// <param name="accountId">Acount id</param>
        /// <param name="userId">currebt user</param>
        /// <returns></returns>
        public async Task<string> ResetPassword(int accountId, int currUserId)
        {
            var now = DateTime.Now;
            var currAccount = await this.account.GetByIdAsync(accountId);
            var password = CreatePassword(6);
            currAccount.MatKhau = Common.MD5Hash(password);
            currAccount.Ngay_DoiMatKhau = now;
            currAccount.SoLan_LoginSai = 0;
            currAccount.Id_NV_CapNhat = currUserId;
            currAccount.Ngay_CapNhat = now;

            await this.account.UpdateAsync(currAccount);
            return password;
        }

        /// <summary>
        /// Generate random a password
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        private string CreatePassword(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }

        /// <summary>
        /// Gets accounts with tree node format
        /// </summary>
        /// <param name="employeeId">Employee id</param>
        /// <returns>Tree nodes</returns>
        public async Task<IEnumerable<TreeNode>> GetTreeNodeAccounts(int employeeId)
        {
            var accounts = await this.account.GetAccountsByEmployeeId(employeeId);
            if (accounts == null)
            {
                return null;
            }

            return accounts.Select(x => new TreeNode
            {
                Children = false,
                Id = "A" + x.Id_NguoiDung,
                Text = x.UserName,
                TypeNode = "Account"
            });
        }
    }
}
