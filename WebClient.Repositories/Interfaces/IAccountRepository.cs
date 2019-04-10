using System.Collections.Generic;
using System.Threading.Tasks;
using WebClient.Core.Entities;

namespace WebClient.Repositories.Interfaces
{
    public interface IAccountRepository : IBaseRepository<Account>
    {
        /// <summary>
        /// Login with the user
        /// </summary>
        /// <param name="username">The username</param>
        /// <param name="password">The password</param>
        /// <returns>A Account of the current user</returns>
        Task<AccountInfo> LoginAsync(string username, string password);

        /// <summary>
        /// update information of account
        /// </summary>
        /// <param name="acc">the account with infor</param>
        /// <returns>void</returns>
        void UpdateInformationAccount(Account acc);

        /// <summary>
        /// get account by username
        /// </summary>
        /// <param name="username">the username</param>
        /// <returns>account</returns>
        Task<Account> GetAccountByUsername(string username);

        Task<bool> ChangePassword(string username, int idNguoiDung, string matKhauCu, string matKhauMoi);

        /// <summary>
        /// Gets employee's accounts
        /// </summary>
        /// <param name="employeeId">Employee id</param>
        /// <returns>List account</returns>
        Task<IEnumerable<Account>> GetAccountsByEmployeeId(int employeeId);

        /// <summary>
        /// Get all account by employee id
        /// </summary>
        /// <param name="employeeId">Employee id</param>
        /// <returns>List account</returns>
        Task<IEnumerable<Account>> GetAllAccountsByEmployeeId(int employeeId);

        /// <summary>
        /// Get account by account code
        /// </summary>
        /// <param name="accountCode">Account code</param>
        /// <returns>A Account instance</returns>
        Task<Account> GetAccountByCode(string accountCode);
    }
}
