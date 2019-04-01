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
    public class PermissionFeatureRepository: IPermissionFeatureRepository
    {
        public async Task AddAsync(Permission_Feature permission_Feature)
        {
            try
            {
                using (var dbConnection = new OracleConnection(WebConfig.ConnectionString))
                {
                    var query = @"INSERT INTO Quyen_ChucNang(Id_Quyen, Id_ChucNang, Id_NV_KhoiTao, Ngay_KhoiTao)
                                VALUES( :idQuyen, :idChucNang, :idNVKhoiTao, :ngayKhoiTao)";
                    await SqlMapper.ExecuteAsync(dbConnection, query, 
                        param: new
                        {
                            idQuyen = permission_Feature.Id_Quyen,
                            idChucNang = permission_Feature.Id_ChucNang,
                            idNVKhoiTao = permission_Feature.Id_NV_KhoiTao,
                            ngayKhoiTao = permission_Feature.Ngay_KhoiTao
                        },
                        commandType: CommandType.Text, commandTimeout: 5000);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task DeleteAsync(Permission_Feature permission_Feature)
        {
            using (var dbConnection = new OracleConnection(WebConfig.ConnectionString))
            {
                var query = @"DELETE FROM Quyen_ChucNang WHERE Id_Quyen = :idQuyen AND Id_ChucNang = :idChucNang";
                await SqlMapper.ExecuteAsync(dbConnection, query,
                    param: new
                    {
                        idQuyen = permission_Feature.Id_Quyen,
                        idChucNang = permission_Feature.Id_ChucNang,
                    },
                    commandType: CommandType.Text, commandTimeout: 5000);
            }
        }

        public async Task<IEnumerable<Permission_Feature>> GetListsByPermissionIdAsync(int permissionId)
        {
            using (var dbConnection = new OracleConnection(WebConfig.ConnectionString))
            {
                var query = @"Select * From quyen_chucnang Where Id_Quyen = :idQuyen";
                var list = await SqlMapper.QueryAsync<Permission_Feature>(dbConnection, query,
                        param: new
                        {
                            idQuyen = permissionId,
                        },
                        commandType: CommandType.Text, commandTimeout: 5000);

                return list;
            }
        }
    }
}
