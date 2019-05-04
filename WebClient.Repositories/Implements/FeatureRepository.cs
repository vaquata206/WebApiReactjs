using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Oracle.ManagedDataAccess.Client;
using WebClient.Core;
using WebClient.Core.Entities;
using WebClient.Repositories.Interfaces;

namespace WebClient.Repositories.Implements
{
    public class FeatureRepository : BaseRepository<Feature>, IFeatureRepository
    {
        /// <summary>
        /// Gets all featuress
        /// </summary>
        /// <param name="id">App id</param>
        /// <returns>A list feature</returns>
        public async Task<IEnumerable<Feature>> GetAllAsync(int id)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("p_chuongtrinh_id", OracleDbType.Int64, ParameterDirection.Input, id);
            dyParam.Add("rsout", OracleDbType.RefCursor, ParameterDirection.Output);

            return await this.DbConnection.QueryAsync<Feature>(QueryResource.Feature_GetAll, param: dyParam, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Get a feature by id
        /// </summary>
        /// <param name="idFeature">Id of feature</param>
        /// <returns>A feature</returns>
        public async Task<Feature> GetFeatureByIdAsync(int idFeature)
        {
            Feature feature = null;
            using (var dbConnection = new OracleConnection(WebConfig.ConnectionString))
            {
                var query = @"SELECT * 
                                FROM chuc_nang WHERE Id_ChucNang = :id AND TINH_TRANG = 1";

                feature = await dbConnection.QueryFirstAsync<Feature>(sql: query, param: new {
                    id = idFeature
                }, commandType: CommandType.Text);
            }

            return feature;
        }

        /// <summary>
        /// Updates a feature
        /// </summary>
        /// <param name="feature">The feature</param>
        /// <returns></returns>
        public async Task UpdateFeatureAsync(Feature feature)
        {
            try
            {
                var dyParam = new OracleDynamicParameters();

                dyParam.Add("P_ID_CN", OracleDbType.Int64, ParameterDirection.Input, feature.Id_ChucNang);
                dyParam.Add("P_Ten_CN", OracleDbType.NVarchar2, ParameterDirection.Input, feature.Ten_ChucNang);
                dyParam.Add("P_Mota_CN", OracleDbType.NVarchar2, ParameterDirection.Input, feature.MoTa_ChucNang);
                dyParam.Add("P_ToolTip_CN", OracleDbType.NVarchar2, ParameterDirection.Input, feature.Tooltip);
                dyParam.Add("P_ID_CN_PR", OracleDbType.Int64, ParameterDirection.Input, feature.Id_ChucNang_Cha);
                dyParam.Add("P_Controller_Name", OracleDbType.NVarchar2, ParameterDirection.Input, feature.Controller_Name);
                dyParam.Add("P_Action_Name", OracleDbType.NVarchar2, ParameterDirection.Input, feature.Action_Name);
                dyParam.Add("P_HienThi_Menu", OracleDbType.Int16, ParameterDirection.Input, feature.HienThi_Menu);

                await this.DbConnection.ExecuteReaderAsync(QueryResource.Feature_UpdateFeature, param: dyParam, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Adds a feature
        /// </summary>
        /// <param name="feature">The feature</param>
        /// <returns>A Task</returns>
        public async Task AddFeatureAsync(Feature feature)
        {
            try
            {
                var dyParam = new OracleDynamicParameters();

                dyParam.Add("P_MA_CN", OracleDbType.NVarchar2, ParameterDirection.Input, feature.Ma_ChucNang);
                dyParam.Add("P_Ten_CN", OracleDbType.NVarchar2, ParameterDirection.Input, feature.Ten_ChucNang);
                dyParam.Add("P_Mota_CN", OracleDbType.NVarchar2, ParameterDirection.Input, feature.MoTa_ChucNang);
                dyParam.Add("P_ToolTip_CN", OracleDbType.NVarchar2, ParameterDirection.Input, feature.Tooltip);
                dyParam.Add("P_ID_CN_PR", OracleDbType.Int64, ParameterDirection.Input, feature.Id_ChucNang_Cha);
                dyParam.Add("P_Controller_Name", OracleDbType.NVarchar2, ParameterDirection.Input, feature.Controller_Name);
                dyParam.Add("P_Action_Name", OracleDbType.NVarchar2, ParameterDirection.Input, feature.Action_Name);
                dyParam.Add("P_HienThi_Menu", OracleDbType.Int16, ParameterDirection.Input, feature.HienThi_Menu);

                await this.DbConnection.ExecuteAsync(QueryResource.Feature_AddFeature, param: dyParam, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Changes a feature's position
        /// </summary>
        /// <param name="parentId">ParentId of the feature</param>
        /// <param name="order">Numerical order of the feature into its parent</param>
        /// <param name="featureId">The feature ids</param>
        /// <returns>A Task</returns>
        public async Task ChangePosition(int parentId, int order, int featureId)
        {
            try
            {
                using (var dbConnection = new OracleConnection(WebConfig.ConnectionString))
                {
                    var query = "ADMIN_FEATURE.CHANGE_POSITION";
                    var dyParam = new OracleDynamicParameters();

                    dyParam.Add("P_ID_CN_PR", OracleDbType.Int64, ParameterDirection.Input, parentId);
                    dyParam.Add("P_STT", OracleDbType.Int64, ParameterDirection.Input, order);
                    dyParam.Add("P_ID_CN", OracleDbType.Int64, ParameterDirection.Input, featureId);

                    await SqlMapper.ExecuteReaderAsync(dbConnection, query, param: dyParam, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Id of the feature that will be deleted
        /// </summary>
        /// <param name="featureId">Id of feature</param>
        /// <returns>A Task</returns>
        public async Task DeleteFeatureAsync(int featureId)
        {
            try
            {
                using (var dbConnection = new OracleConnection(WebConfig.ConnectionString))
                {
                    var query = "ADMIN_FEATURE.DELETE_FEATURE";
                    var dyParam = new OracleDynamicParameters();

                    dyParam.Add("P_ID_CN", OracleDbType.Int64, ParameterDirection.Input, featureId);
                    await SqlMapper.ExecuteReaderAsync(dbConnection, query, param: dyParam, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Id of the permission
        /// </summary>
        /// <param name="permissionId">Id of permission</param>
        /// <returns>A list feature</returns>
        public async Task<IEnumerable<Feature>> GetFeaturesByPermissionId(int permissionId)
        {
            try
            {
                using (var dbConnection = new OracleConnection(WebConfig.ConnectionString))
                {
                    var query = @"SELECT cn.ID_CHUCNANG, cn.TEN_CHUCNANG, cn.MOTA_CHUCNANG, cn.TOOLTIP, cn.ID_CHUCNANG_CHA, cn.CONTROLLER_NAME, cn.ACTION_NAME, cn.THU_TU 
                                FROM chuc_nang cn JOIN QUYEN_CHUCNANG ON cn.ID_CHUCNANG = QUYEN_CHUCNANG.ID_CHUCNANG
                                WHERE cn.TINH_TRANG = 1 AND QUYEN_CHUCNANG.ID_QUYEN = 1";
                    var list = await SqlMapper.QueryAsync<Feature>(dbConnection, query, commandType: CommandType.Text, commandTimeout: 5000);
                    return list;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<Feature>> GetFeaturesOfPermissionsByAccountId(int accountId)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("p_id_nd", OracleDbType.Int64, ParameterDirection.Input, accountId);
            dyParam.Add("rsout", OracleDbType.RefCursor, ParameterDirection.Output);

            return await this.DbConnection.QueryAsync<Feature>(
                QueryResource.Feature_FeaturePermissionAccount, 
                param: dyParam, 
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Feature>> GetFeaturesNotBelongPermissionsByAccountId(int accountId)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("p_id_nd", OracleDbType.Int64, ParameterDirection.Input, accountId);
            dyParam.Add("rsout", OracleDbType.RefCursor, ParameterDirection.Output);

            return await this.DbConnection.QueryAsync<Feature>(
                QueryResource.Feature_FeatureNoPermissionAccount, 
                param: dyParam, 
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Feature>> GetFeaturesUser(int userId)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("p_id_nd", OracleDbType.Int64, ParameterDirection.Input, userId);
            dyParam.Add("rsout", OracleDbType.RefCursor, ParameterDirection.Output);

            return await this.DbConnection.QueryAsync<Feature>(QueryResource.Feature_Get_Menu_Feature, dyParam, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Checks the user is access to feature whose link like the "path"
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="controlerName">The controller name</param>
        /// <param name="actionName">The action name</param>
        /// <param name="isModeUri">Is mode uri</param>
        /// <returns>true if the user is accessed</returns>
        public async Task<bool> IsAccessedToTheFeature(int userId, string controlerName, string actionName, bool isModeUri)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("p_userId", OracleDbType.Int64, ParameterDirection.Input, userId);
            dyParam.Add("p_controllerName", OracleDbType.Varchar2, ParameterDirection.Input, controlerName);
            dyParam.Add("p_actionName", OracleDbType.Varchar2, ParameterDirection.Input, actionName);
            dyParam.Add("p_isModeUri", OracleDbType.Int64, ParameterDirection.Input, isModeUri);
            dyParam.Add("o_isAccess", OracleDbType.Int64, ParameterDirection.Output);

            await this.DbConnection.ExecuteAsync(QueryResource.Feature_IsAccessedFeature, dyParam, commandType: CommandType.StoredProcedure);
            return int.Parse(dyParam.GetByName("o_isAccess").Value.ToString()) == 1;
        }
    }
}
