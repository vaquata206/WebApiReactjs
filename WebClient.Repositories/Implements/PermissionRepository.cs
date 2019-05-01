using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Oracle.ManagedDataAccess.Client;
using WebClient.Core;
using WebClient.Core.Entities;
using WebClient.Repositories.Interfaces;

namespace WebClient.Repositories.Implements
{
    public class PermissionRepository : BaseRepository<Permission>, IPermissionRepository
    {
        /// <summary>
        /// Get list permission
        /// </summary>
        /// <returns>List permission</returns>
        public async Task<IEnumerable<Permission>> GetPermissions()
        {
            using (var dbConnection = new OracleConnection(WebConfig.ConnectionString))
            {
                var query = @"SELECT Id_Quyen, Ma_Quyen, Ten_Quyen, Tinh_Trang, Ghi_Chu FROM DM_Quyen WHERE Tinh_Trang = 1";

                var permissions = await dbConnection.QueryAsync<Permission>(query, commandType: System.Data.CommandType.Text);
                return permissions;
            }
        }

        /// <summary>
        /// Delete permission
        /// </summary>
        /// <param name="permissionId">Permission id</param>
        /// <returns></returns>
        public async Task DeleteAsync(int permissionId)
        {
            try
            {
                using (var dbConnection = new OracleConnection(WebConfig.ConnectionString))
                {
                    var query = @"ADMIN_QUYEN.DELETE_QUYEN";
                    var dyParam = new OracleDynamicParameters();

                    dyParam.Add("P_ID_QUYEN", OracleDbType.Int64, ParameterDirection.Input, permissionId);
                    var permissions = await dbConnection.QueryAsync<Permission>(query, param: dyParam, commandType: System.Data.CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Sets departments that the account is avaiabled work
        /// </summary>
        /// <param name="accountId">Account id</param>
        /// <param name="departmentIds">Department ids</param>
        /// <param name="handlerId">Id of user who is performing this action</param>
        public async Task SetDepartmentsAsync(int accountId, int[] departmentIds, int handlerId)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("p_id_nd", OracleDbType.Int64, ParameterDirection.Input, accountId);
            if (departmentIds != null && departmentIds.Length > 0) {
                dyParam.Add("p_id_dvs", OracleDbType.Varchar2, ParameterDirection.Input, string.Join(",", departmentIds));
            }
            else
            {
                dyParam.Add("p_id_dvs", OracleDbType.Varchar2, ParameterDirection.Input);
            }

            dyParam.Add("p_id_nv_khoitao", OracleDbType.Varchar2, ParameterDirection.Input, handlerId);

            await this.DbConnection.ExecuteAsync(
                QueryResource.Permission_SetDepartments,
                param: dyParam,
                commandType: CommandType.StoredProcedure);
        }
    }
}
