using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Oracle.ManagedDataAccess.Client;
using WebClient.Core.Entities;
using WebClient.Repositories.Interfaces;

namespace WebClient.Repositories.Implements
{
    public class EmployeePermissionRepository : BaseRepository<Employee_Permission>, IEmployeePermissionRepository
    {
        /// <summary>
        /// Set permission for a user
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="userId"></param>
        /// <param name="handler"></param>
        /// <returns></returns>
        public async Task SetPermissionsForUser(IEnumerable<int> ids, int userId, int handler)
        {
            try
            {
                var dyParam = new OracleDynamicParameters();
                dyParam.Add("P_IDS", OracleDbType.Varchar2, ParameterDirection.Input, string.Join(",", ids));
                dyParam.Add("P_ID_ND", OracleDbType.Int64, ParameterDirection.Input, userId);
                dyParam.Add("P_ID_NV_TT", OracleDbType.Int64, ParameterDirection.Input, handler);
                await this.DbConnection.ExecuteAsync(
                    QueryResource.AccPermission_SetPermissions, 
                    param: dyParam, 
                    commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        /// <summary>
        /// Get id of permissions that a user can accepts
        /// </summary>
        /// <param name="userId">user id</param>
        /// <returns>permission ids</returns>
        public async Task<IEnumerable<int>> GetIdPermissionsOfUser(int userId, int appId)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("P_ID_ND", OracleDbType.Int64, ParameterDirection.Input, userId);
            dyParam.Add("P_ID_CHUONGTRINH", OracleDbType.Int64, ParameterDirection.Input, appId);
            dyParam.Add("RSOUT", OracleDbType.RefCursor, ParameterDirection.Output);
            return await this.DbConnection.QueryAsync<int>(
                QueryResource.AccPermission_GetPermissionIdsUser, 
                param: dyParam, 
                commandType: CommandType.StoredProcedure);
        }

        public async Task SetFeaturesForEmployee(IEnumerable<int> ids, int idEmployee, int idUser)
        {
            try
            {
                var query = "ADMIN_NHANVIEN_QUYEN.SET_FEATURES";
                var dyParam = new OracleDynamicParameters();
                dyParam.Add("P_ID_CNS", OracleDbType.Varchar2, ParameterDirection.Input, string.Join(",", ids));
                dyParam.Add("P_ID_NV", OracleDbType.Int64, ParameterDirection.Input, idEmployee);
                dyParam.Add("P_ID_USER", OracleDbType.Int64, ParameterDirection.Input, idUser);
                await this.DbConnection.ExecuteAsync(query, param: dyParam, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
