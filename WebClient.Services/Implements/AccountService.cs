using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using WebClient.Core.Entities;
using WebClient.Core.Helper;
using WebClient.Core.Messages;
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
        private IAccountRepository accountRepository;

        /// <summary>
        /// department repository
        /// </summary>
        private IDepartmentRepository departmentRepository;

        /// <summary>
        /// Employee repository
        /// </summary>
        private IEmployeeRepository employeeRepository;

        /// <summary>
        /// Rabbit mqService
        /// </summary>
        private IRabbitMQService rabbitMQService;

        /// <summary>
        /// Mapper service
        /// </summary>
        private IMapper mapper;

        /// <summary>
        /// A constructor
        /// </summary>
        /// <param name="account">account repository</param>
        /// <param name="departmentRepository">department repository</param>
        public AccountService(
            IAccountRepository account, 
            IDepartmentRepository departmentRepository, 
            IEmployeeRepository employeeRepository,
            IRabbitMQService rabbitMQService,
            IMapper mapper)
        {
            this.accountRepository = account;
            this.departmentRepository = departmentRepository;
            this.employeeRepository = employeeRepository;
            this.rabbitMQService = rabbitMQService;
            this.mapper = mapper;
        }

        /// <summary>
        /// Login the user
        /// </summary>
        /// <param name="username">The username of account</param>
        /// <param name="password">The user's password</param>
        public async Task<AccountInfo> LoginAsync(string username, string password)
        {
            var account =  await this.accountRepository.LoginAsync(username, password);
            return account;
        }

        /// <summary>
        /// update information account
        /// </summary>
        /// <param name="account">account with new information</param>
        public void UpdateInformationAccount(Account acc)
        {
            this.accountRepository.UpdateInformationAccount(acc);
        }

        public async Task<Account> GetAccountByUsername(string username)
        {
            return await this.accountRepository.GetAccountByUsername(username);
        }

        public async Task<bool> ChangePassword(string username, int idNguoiDung, string matKhauCu, string matKhauMoi)
        {
            return await this.accountRepository.ChangePassword(username, idNguoiDung, matKhauCu, matKhauMoi);
        }

        /// <summary>
        /// Gets employee's accounts
        /// </summary>
        /// <param name="employeeId">Employee id</param>
        /// <returns>List account</returns>
        public async Task<IEnumerable<Account>> GetAccountsByEmployeeId(int employeeId)
        {
            return await this.accountRepository.GetAccountsByEmployeeId(employeeId);
        }

        /// <summary>
        /// Create a new account for a employee
        /// </summary>
        /// <param name="accountVM">Account VM</param>
        /// <param name="currentUserId">Current user id</param>
        /// <returns>A void task</returns>
        public async Task<Account> CreateAccount(AccountVM accountVM, int currentUserId)
        {
            var employee = await this.employeeRepository.GetEmployeeByCode(accountVM.Ma_NhanVien);
            if (employee == null)
            {
                throw new Exception("Nhân viên không tồn tại");
            }

            var account = new Account
            {
                Id_NhanVien = employee.Id_NhanVien,
                UserName = accountVM.UserName,
                MatKhau = Common.MD5Hash(accountVM.MatKhau),
                Id_NV_KhoiTao = currentUserId,
                Ngay_KhoiTao = DateTime.Now,
                Ma_NguoiDung = "U" + DateTime.Now.ToString("yyyyMMddHHmmss"),
                Tinh_Trang = 1,
                Quan_Tri = 0,
                Id_VaiTro = 3,
                Ngay_CapNhat = DateTime.Now
            };

            await this.accountRepository.AddAsync(account);
            await this.PublishCreatingAccount(account);

            return account;
        }

        /// <summary>
        /// Delete a account
        /// </summary>
        /// <param name="accountCode">Account Code</param>
        /// <param name="currUserId">Current user id</param>
        /// <returns>A account</returns>
        public async Task<Account> DeleteAccount(string accountCode, int currUserId)
        {
            var account = await this.accountRepository.GetAccountByCode(accountCode);
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
            account.Id_NV_CapNhat = currUserId;

            await this.accountRepository.UpdateAsync(account);
            await this.PublishUpdatingAccount(account.Ma_NguoiDung, account);

            return account;
        }

        /// <summary>
        /// Reset password of a account
        /// </summary>
        /// <param name="accountCode">Acount code</param>
        /// <param name="userId">currebt user</param>
        /// <returns></returns>
        public async Task<string> ResetPassword(string accountCode, int currUserId)
        {
            var now = DateTime.Now;
            var currAccount = await this.accountRepository.GetAccountByCode(accountCode);
            if (currAccount == null)
            {
                throw new Exception("Tài khoản không tồn tại");
            }

            var password = CreatePassword(6);
            currAccount.MatKhau = Common.MD5Hash(password);
            currAccount.Ngay_DoiMatKhau = now;
            currAccount.SoLan_LoginSai = 0;
            currAccount.Id_NV_CapNhat = currUserId;
            currAccount.Ngay_CapNhat = now;

            await this.accountRepository.UpdateAsync(currAccount);
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
            var accounts = await this.accountRepository.GetAccountsByEmployeeId(employeeId);
            if (accounts == null)
            {
                return null;
            }

            return accounts.Select(x => new TreeNode
            {
                Children = false,
                Id = x.Id_NguoiDung,
                Title = x.UserName,
                TypeNode = "Account"
            });
        }

        /// <summary>
        /// Publish creating account
        /// </summary>
        /// <param name="account">A account instance</param>
        /// <returns>A void task</returns>
        private async Task PublishCreatingAccount(Account account)
        {
            var message = this.mapper.Map<AccountMessage>(account);
            var employee = await this.employeeRepository.GetEmployeeById(account.Id_NhanVien);
            if (employee != null)
            {
                message.Ma_NhanVien = employee.Ma_NhanVien;
            }

            this.rabbitMQService.Publish(
                message,
                RabbitActionTypes.Create,
                RabbitEntities.Account
                );
        }

        /// <summary>
        /// Publish updating account
        /// </summary>
        /// <param name="accountCode">Account code</param>
        /// <param name="account">Account instance</param>
        /// <returns></returns>
        private async Task PublishUpdatingAccount(string accountCode, Account account)
        {
            var message = this.mapper.Map<AccountMessage>(account);
            var employee = await this.employeeRepository.GetEmployeeById(account.Id_NhanVien);
            if (employee != null)
            {
                message.Ma_NhanVien = employee.Ma_NhanVien;
            }

            message.Ma_NguoiDung_Cu = accountCode;
            this.rabbitMQService.Publish(
                message,
                RabbitActionTypes.Update,
                RabbitEntities.Account
                );
        }
    }
}
