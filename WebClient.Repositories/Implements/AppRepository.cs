using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Oracle.ManagedDataAccess.Client;
using WebClient.Core.Entities;
using WebClient.Core.Responses;
using WebClient.Repositories.Interfaces;

namespace WebClient.Repositories.Implements
{
    public class AppRepository : BaseRepository<App>, IAppRepository
    {
        /// <summary>
        /// Get all app
        /// </summary>
        /// <returns>List app</returns>
        public async Task<IEnumerable<AppResponse>> GetAll()
        {
            var dyparam = new OracleDynamicParameters();
            dyparam.Add("rsout", OracleDbType.RefCursor, ParameterDirection.Output);
            return await this.DbConnection.QueryAsync<AppResponse>(
                QueryResource.App_GetAll, 
                param: dyparam, 
                commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Get apps that the user can accessed
        /// </summary>
        /// <param name="handler">Who doing this action</param>
        /// <returns>List app</returns>
        public async Task<IEnumerable<AppResponse>> GetUserApps(int handler)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("p_id_nd", OracleDbType.Int64, ParameterDirection.Input, handler);
            dyParam.Add("rsout", OracleDbType.RefCursor, ParameterDirection.Output);

            return await this.DbConnection.QueryAsync<AppResponse>(
                QueryResource.App_GetUserApps, 
                param: dyParam, 
                commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Sets apps that the user can accessed
        /// </summary>
        /// <param name="id_NguoiDung">Id user</param>
        /// <param name="idApps">Id Apps</param>
        /// <param name="handler">Who doing this action</param>
        /// <returns>A void task</returns>
        public async Task SetPermission(int id_NguoiDung, string idApps, int handler)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("p_id_nd", OracleDbType.Int64, ParameterDirection.Input, id_NguoiDung);
            dyParam.Add("p_id_cts", OracleDbType.Varchar2, ParameterDirection.Input, idApps);
            dyParam.Add("p_id_thuchien", OracleDbType.Int64, ParameterDirection.Input, handler);

            await this.DbConnection.QueryAsync<AppResponse>(
                QueryResource.App_SetPermission,
                param: dyParam,
                commandType: CommandType.StoredProcedure);
        }
    }
}
