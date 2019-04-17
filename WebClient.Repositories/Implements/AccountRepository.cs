using Dapper;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WebClient.Core;
using WebClient.Core.Entities;
using WebClient.Core.Helper;
using WebClient.Repositories.Interfaces;

namespace WebClient.Repositories.Implements
{

    /// <summary>
    /// Account repository
    /// </summary>
    public class AccountRepository : BaseRepository<Account>, IAccountRepository
    {
        /// <summary>
        /// Login the user
        /// </summary>
        /// <param name="username">The usename</param>
        /// <param name="password">The password</param>
        /// <returns>Access token</returns>
        public async Task<AccountInfo> LoginAsync(string username, string password)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("p_username", OracleDbType.Varchar2, ParameterDirection.Input, username);
            dyParam.Add("p_password", OracleDbType.Varchar2, ParameterDirection.Input, Common.MD5Hash(password));
            dyParam.Add("RSOUT", OracleDbType.RefCursor, ParameterDirection.Output);
            return await this.DbConnection.QueryFirstOrDefaultAsync<AccountInfo>(QueryResource.Account_Login, param: dyParam, commandType: CommandType.StoredProcedure, commandTimeout: 1200);
        }

        /// <summary>
        /// update information of account
        /// </summary>
        /// <param name="acc">the account with infor</param>
        /// <returns>true: update success -- false: update fail</returns>
        public async void UpdateInformationAccount(Account acc)
        {
            Account account = null;
            try
            {
                using (IDbConnection dbConnection = new OracleConnection(WebConfig.ConnectionString))
                {
                    var dyParam = new OracleDynamicParameters();
                    //dyParam.Add("P_HOTEN", OracleDbType.Varchar2, ParameterDirection.Input, acc.Ho_ten);
                    dyParam.Add("P_ID_NHANVIEN", OracleDbType.Varchar2, ParameterDirection.Input, 370);
                    dyParam.Add("RSOUT", OracleDbType.RefCursor, ParameterDirection.Output);
                    //var query = ManagerQueryRecource.ResourceManager.GetString("GET_ACCOUNT");
                    var query = "BLDT_ACCOUNT.CAPNHAT_NHANVIEN1";
                    var obj = await SqlMapper.QueryAsync<Account>(dbConnection, query, param: dyParam, commandType: CommandType.StoredProcedure);
                    account = (obj != null || obj.Count() == 0) ? obj.FirstOrDefault() : null;
                    //account.Ho_ten = acc.Ho_ten;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// get account by username
        /// </summary>
        /// <param name="account">username</param>
        /// <returns>account</returns>
        public async Task<Account> GetAccountByUsername(string username)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("p_username", OracleDbType.Varchar2, ParameterDirection.Input, username);
            dyParam.Add("rs", OracleDbType.RefCursor, ParameterDirection.Output);
            var query = QueryResource.Account_GetAccountByUsername;
            return await this.DbConnection.QueryFirstOrDefaultAsync<Account>(query, param: dyParam, commandType: CommandType.StoredProcedure);
        }

        public async Task<bool> ChangePassword(string username, int idNguoiDung, string matKhauCu, string matKhauMoi)
        {
            using (var dbConnection = new OracleConnection(WebConfig.ConnectionString))
            {
                var dyParam = new OracleDynamicParameters();
                dyParam.Add("p_username", OracleDbType.Varchar2, ParameterDirection.Input, username);
                dyParam.Add("p_id_nhanvien", OracleDbType.Int64, ParameterDirection.Input, idNguoiDung);
                dyParam.Add("p_current_password", OracleDbType.Varchar2, ParameterDirection.Input, Common.MD5Hash(matKhauCu));
                dyParam.Add("p_new_password", OracleDbType.Varchar2, ParameterDirection.Input, Common.MD5Hash(matKhauMoi));
                dyParam.Add("rs", OracleDbType.Int16, ParameterDirection.Output);
                var query = QueryResource.Account_ChangePassword;
                await SqlMapper.QueryAsync<Account>(dbConnection, query, param: dyParam, commandType: CommandType.StoredProcedure);
                var rs = int.Parse(dyParam.GetByName("rs").Value.ToString());
                return rs == 1 ? true : false;
            }
        }

        /// <summary>
        /// Gets employee's accounts
        /// </summary>
        /// <param name="employeeId">Employee id</param>
        /// <returns>List account</returns>
        public async Task<IEnumerable<Account>> GetAccountsByEmployeeId(int employeeId)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("p_employeeId", OracleDbType.Int64, ParameterDirection.Input, employeeId);
            dyParam.Add("rs", OracleDbType.RefCursor, ParameterDirection.Output);
            return await this.DbConnection.QueryAsync<Account>(QueryResource.Account_GetAccountsByEmployeeId, dyParam, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Get all account by employee id
        /// </summary>
        /// <param name="employeeId">Employee id</param>
        /// <returns>List account</returns>
        public async Task<IEnumerable<Account>> GetAllAccountsByEmployeeId(int employeeId)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("p_employeeId", OracleDbType.Int64, ParameterDirection.Input, employeeId);
            dyParam.Add("rs", OracleDbType.RefCursor, ParameterDirection.Output);
            return await this.DbConnection.QueryAsync<Account>(QueryResource.Account_GetAllAccountsByEmployeeId, dyParam, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Get account by account code
        /// </summary>
        /// <param name="accountCode">Account code</param>
        /// <returns>A Account instance</returns>
        public async Task<Account> GetAccountByCode(string accountCode)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("p_ma_nguoidung", OracleDbType.Varchar2, ParameterDirection.Input, accountCode);
            dyParam.Add("rsout", OracleDbType.RefCursor, ParameterDirection.Output);

            return await this.DbConnection.QueryFirstOrDefaultAsync<Account>(
                QueryResource.Account_GetAccountByCode,
                param: dyParam,
                commandType: CommandType.StoredProcedure
                );
        }
    }
}
