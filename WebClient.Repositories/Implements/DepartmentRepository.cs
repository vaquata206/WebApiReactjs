using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Oracle.ManagedDataAccess.Client;
using WebClient.Core.Entities;
using WebClient.Core.ViewModels;
using WebClient.Repositories.Interfaces;

namespace WebClient.Repositories.Implements
{
    public class DepartmentRepository : BaseRepository<Department>, IDepartmentRepository
    {
        /// <summary>
        /// Get department by department code
        /// </summary>
        /// <param name="departmentCode">Department code</param>
        /// <returns>A department instance</returns>
        public async Task<Department> GetDepartmentByCode(string departmentCode)
        {
            var query = "select * from don_vi where ma_donvi = :departmentCode";
            return await this.DbConnection.QueryFirstOrDefaultAsync<Department>(
                query,
                param: new { departmentCode = departmentCode },
                commandType: CommandType.Text);
        }

        /// <summary>
        /// Insert new Department
        /// </summary>
        /// <param name="department">the new Department</param>
        /// <returns>the task</returns>
        public async Task<Department> AddDepartmentAsync(Department department)
        {
            try
            {
                var query = QueryResource.Department_Insert;
                var dyParam = new OracleDynamicParameters();
                dyParam.Add("p_ma_donvi", OracleDbType.Varchar2, ParameterDirection.Input, department.Ma_DonVi);
                dyParam.Add("p_ten_donvi", OracleDbType.Varchar2, ParameterDirection.Input, department.Ten_DonVi);
                dyParam.Add("p_dia_chi", OracleDbType.Varchar2, ParameterDirection.Input, department.Dia_Chi);
                dyParam.Add("p_masothue", OracleDbType.Varchar2, ParameterDirection.Input, department.MaSoThue);
                dyParam.Add("p_dien_thoai", OracleDbType.Varchar2, ParameterDirection.Input, department.Dien_Thoai);
                dyParam.Add("p_website", OracleDbType.Varchar2, ParameterDirection.Input, department.Website);
                dyParam.Add("p_tennguoi_daidien", OracleDbType.Varchar2, ParameterDirection.Input, department.TenNguoi_DaiDien);
                dyParam.Add("p_loai_donvi", OracleDbType.Int64, ParameterDirection.Input, department.Loai_DonVi);
                dyParam.Add("p_id_nv_khoitao", OracleDbType.Int64, ParameterDirection.Input, department.Id_NV_KhoiTao);
                dyParam.Add("p_ngay_khoitao", OracleDbType.Date, ParameterDirection.Input, department.Ngay_KhoiTao);
                dyParam.Add("p_id_dv_cha", OracleDbType.Int64, ParameterDirection.Input, department.Id_DV_Cha);
                dyParam.Add("p_cap_donvi", OracleDbType.Int64, ParameterDirection.Input, department.Cap_DonVi);
                dyParam.Add("p_ghi_chu", OracleDbType.Varchar2, ParameterDirection.Input, department.Ghi_Chu);
                dyParam.Add("rsout", OracleDbType.RefCursor, ParameterDirection.Output);
                return await this.DbConnection.QueryFirstOrDefaultAsync<Department>(query, param: dyParam, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Gets department that accessed by the handler with department id
        /// </summary>
        /// <param name="idDonVi">department id</param>
        /// <param name="handler">who is doing this action</param>
        /// <returns></returns>
        public async Task<Department> GetDepartmentByIdAsync(int idDonVi, int handler)
        {
            try
            {
                var dyParam = new OracleDynamicParameters();
                dyParam.Add("P_ID_DONVI", OracleDbType.Int64, ParameterDirection.Input, idDonVi);
                dyParam.Add("P_ID_ND_THUCHIEN", OracleDbType.Int64, ParameterDirection.Input, handler);
                dyParam.Add("RSOUT", OracleDbType.RefCursor, ParameterDirection.Output);
                var query = QueryResource.Department_Get_By_Id;
                return await this.DbConnection.QueryFirstOrDefaultAsync<Department>(
                    query, 
                    param: dyParam, 
                    commandType: CommandType.StoredProcedure, 
                    commandTimeout: 5000);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Gets all children by parent Id
        /// </summary>
        /// <param name="parentId">Parent department id</param>
        /// <returns>List department</returns>
        public async Task<IEnumerable<Department>> GetDepartmentsByIdParent(int parentId)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("p_id_dv_cha", OracleDbType.Int64, ParameterDirection.Input, parentId);
            dyParam.Add("rsout", OracleDbType.RefCursor, ParameterDirection.Output);
            return await this.DbConnection.QueryAsync<Department>(
                QueryResource.Department_GetAllChildDepartments,
                param: dyParam,
                commandType: System.Data.CommandType.StoredProcedure);
        }

        /// <summary>
        /// Updates a Department
        /// </summary>
        /// <param name="Department">The Department</param>
        /// <returns>the task</returns>
        public async Task<Department> UpdateDepartmentAsync(Department department)
        {
            try
            {
                var query = QueryResource.Department_Update;
                var dyParam = new OracleDynamicParameters();
                dyParam.Add("p_id_donvi", OracleDbType.Int64, ParameterDirection.Input, department.Id_DonVi);
                dyParam.Add("p_ten_donvi", OracleDbType.NVarchar2, ParameterDirection.Input, department.Ten_DonVi);
                dyParam.Add("p_dia_chi", OracleDbType.NVarchar2, ParameterDirection.Input, department.Dia_Chi);
                dyParam.Add("p_masothue", OracleDbType.Varchar2, ParameterDirection.Input, department.MaSoThue);
                dyParam.Add("p_dien_thoai", OracleDbType.NVarchar2, ParameterDirection.Input, department.Dien_Thoai);
                dyParam.Add("p_website", OracleDbType.NVarchar2, ParameterDirection.Input, department.Website);
                dyParam.Add("p_tennguoi_daidien", OracleDbType.NVarchar2, ParameterDirection.Input, department.TenNguoi_DaiDien);
                dyParam.Add("p_loai_donvi", OracleDbType.Int64, ParameterDirection.Input, department.Loai_DonVi);
                dyParam.Add("p_id_nv_capnhap", OracleDbType.Int64, ParameterDirection.Input, department.Id_NV_CapNhat);
                dyParam.Add("p_ngay_capnhap", OracleDbType.Date, ParameterDirection.Input, department.Ngay_CapNhat);
                dyParam.Add("p_id_dv_cha", OracleDbType.Int64, ParameterDirection.Input, department.Id_DV_Cha);
                dyParam.Add("p_cap_donvi", OracleDbType.Int64, ParameterDirection.Input, department.Cap_DonVi);
                dyParam.Add("p_ghi_chu", OracleDbType.Varchar2, ParameterDirection.Input, department.Ghi_Chu);
                dyParam.Add("rsout", OracleDbType.RefCursor, ParameterDirection.Output);

                return await this.DbConnection.QueryFirstOrDefaultAsync<Department>(query, param: dyParam, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Update email
        /// </summary>
        /// <param name="department">Department instance</param>
        /// <returns>Department instance</returns>
        public async Task<Department> UpdateEmail(Department department)
        {
            try
            {
                var query = QueryResource.Department_UpdateEmail;
                var dyParam = new OracleDynamicParameters();
                dyParam.Add("p_id_donvi", OracleDbType.Int64, ParameterDirection.Input, department.Id_DonVi);
                dyParam.Add("p_email", OracleDbType.Varchar2, ParameterDirection.Input, department.Email);
                dyParam.Add("p_smtp_email", OracleDbType.Varchar2, ParameterDirection.Input, department.SMTP_Email);
                dyParam.Add("p_port_email", OracleDbType.Int64, ParameterDirection.Input, department.Port_Email);
                dyParam.Add("p_account_email", OracleDbType.Varchar2, ParameterDirection.Input, department.Account_Email);
                dyParam.Add("p_pass_email", OracleDbType.Varchar2, ParameterDirection.Input, department.Pass_Email);
                dyParam.Add("p_id_nv_capnhat", OracleDbType.Int64, ParameterDirection.Input, department.Id_NV_CapNhat);
                dyParam.Add("rsout", OracleDbType.RefCursor, ParameterDirection.Output);
                return await this.DbConnection.QueryFirstOrDefaultAsync<Department>(query, param: dyParam, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Gets child nodes by parent Id
        /// </summary>
        /// <param name="parentId">Parent Id</param>
        /// <param name="handler">Who is doing this action</param>
        /// <returns>List node</returns>
        public async Task<IEnumerable<DepartmentNodeVM>> GetChildNodes(int parentId, int handler)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("p_id_dv_cha", OracleDbType.Int64, ParameterDirection.Input, parentId);
            dyParam.Add("p_id_nd_thuchien", OracleDbType.Int64, ParameterDirection.Input, handler);
            dyParam.Add("rsout", OracleDbType.RefCursor, ParameterDirection.Output);

            return await this.DbConnection.QueryAsync<DepartmentNodeVM>(
                QueryResource.Department_GetChildNodes,
                param: dyParam,
                commandType: CommandType.StoredProcedure
                );
        }

        /// <summary>
        /// Get all department that the user is accessed
        /// </summary>
        /// <param name="handler">Who is doing this action</param>
        /// <returns>List department</returns>
        public async Task<IEnumerable<Department>> GetAllDepartmentAccessed(int handler)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("p_id_nd", OracleDbType.Int64, ParameterDirection.Input, handler);
            dyParam.Add("rsout", OracleDbType.RefCursor, ParameterDirection.Output);

            return await this.DbConnection.QueryAsync<Department>(
                QueryResource.Department_GetAllDepartmentAccessed,
                param: dyParam,
                commandType: CommandType.StoredProcedure
                );
        }
    }
}

