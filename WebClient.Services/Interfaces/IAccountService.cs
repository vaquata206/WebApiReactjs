using System.Collections.Generic;
using System.Threading.Tasks;
using WebClient.Core.Entities;
using WebClient.Core.ViewModels;

namespace WebClient.Services.Interfaces
{
    /// <summary>
    /// Account service
    /// </summary>
    public interface IAccountService
    {
        /// <summary>
        /// Login the user
        /// </summary>
        /// <param name="username">The username of account</param>
        /// <param name="password">The user's password</param>
        Task<AccountInfo> LoginAsync(string username, string password);

        /// <summary>
        /// update information account
        /// </summary>
        /// <param name="acc">account with informaition</param>
        void UpdateInformationAccount(Account acc);

        Task<Account> GetAccountByUsername(string username);

        /// <summary>
        /// Change password
        /// </summary>
        /// <param name="username">the username</param>
        /// <param name="idNguoiDung">id nhanvien current</param>
        /// <param name="matKhauCu">the current password</param>
        /// <param name="matKhauMoi">the new password</param>
        /// <returns>TRUE: success, FALSE: The current password fail.</returns>
        Task<bool> ChangePassword(string username,int idNguoiDung, string matKhauCu, string matKhauMoi);

        /// <summary>
        /// Gets employee's accounts
        /// </summary>
        /// <param name="employeeId">Employee id</param>
        /// <returns>List account</returns>
        Task<IEnumerable<Account>> GetAccountsByEmployeeId(int employeeId);

        /// <summary>
        /// Create a new account for a employee
        /// </summary>
        /// <param name="accountVM">Account VM</param>
        /// <param name="currentUserId">Current user id</param>
        /// <returns>A void task</returns>
        Task<Account> CreateAccount(AccountVM accountVM, int currentUserId);

        /// <summary>
        /// Delete a account
        /// </summary>
        /// <param name="accountCode">Account code</param>
        /// <param name="currUserId">Current user id</param>
        /// <returns>A account</returns>
        Task<Account> DeleteAccount(string accountCode, int currUserId);

        /// <summary>
        /// Reset password of a account
        /// </summary>
        /// <param name="accountCode">Acount code</param>
        /// <param name="userId">currebt user</param>
        /// <returns></returns>
        Task<string> ResetPassword(string accountCode, int currUserId);

        /// <summary>
        /// Gets accounts with tree node format
        /// </summary>
        /// <param name="employeeId">Employee id</param>
        /// <returns>Tree nodes</returns>
        Task<IEnumerable<TreeNode>> GetTreeNodeAccounts(int employeeId);
    }
}
