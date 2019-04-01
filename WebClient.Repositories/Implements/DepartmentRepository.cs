using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Oracle.ManagedDataAccess.Client;
using WebClient.Core.Entities;
using WebClient.Repositories.Interfaces;

namespace WebClient.Repositories.Implements
{
    public class DepartmentRepository : BaseRepository<Department>, IDepartmentRepository
    {
        /// <summary>
        /// Insert new Department
        /// </summary>
        /// <param name="department">the new Department</param>
        /// <returns>the task</returns>
        public async Task AddDepartmentAsync(Department department)
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
                dyParam.Add("p_ghi_chu", OracleDbType.NVarchar2, ParameterDirection.Input, department.Ghi_Chu);
                await this.DbConnection.QueryAsync(query, param: dyParam, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Id of the Department that will be deleted
        /// </summary>
        /// <param name="iddonvi">Id of Department</param>
        /// <returns>return 0: exist department children and employees
        ///          return 1: exist department children
        ///          return 2: exist employees
        ///          return 3: delete success
        /// </returns>
        public async Task<int> DeleteDepartmentAsync(int idDonVi)
        {
            try
            {
                var query = QueryResource.Department_Delete;
                var dyParam = new OracleDynamicParameters();
                dyParam.Add("p_id_donvi", OracleDbType.Int64, ParameterDirection.Input, idDonVi);
                dyParam.Add("rs", OracleDbType.Int64, ParameterDirection.Output);
                await this.DbConnection.QueryAsync(query, param: dyParam, commandType: CommandType.StoredProcedure);
                return int.Parse(dyParam.GetByName("rs").Value.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get all units
        /// </summary>
        /// <returns>List units</returns>
        public async Task<IEnumerable<Department>> GetAllAsync()
        {
            try
            {
                var dyParam = new OracleDynamicParameters();
                dyParam.Add("RSOUT", OracleDbType.RefCursor, ParameterDirection.Output);
                var query = QueryResource.Department_GetALL;
                return await this.DbConnection.QueryAsync<Department>(
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

        public async Task<Department> GetDepartmentByIdAsync(int idDonVi)
        {
            try
            {
                var dyParam = new OracleDynamicParameters();
                dyParam.Add("P_ID_DONVI", OracleDbType.Int64, ParameterDirection.Input, idDonVi);
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
        /// Gets children that are controlled by the account by parent Id
        /// </summary>
        /// <param name="parentId">parent department id</param>
        /// <param name="accountId">Account id</param>
        /// <returns>List department</returns>
        public async Task<IEnumerable<Department>> GetDepartmentsByIdParent(int idParent, int accountId)
        {
            try
            {
                var dyParam = new OracleDynamicParameters();
                dyParam.Add("p_id_dv_cha", OracleDbType.Int64, ParameterDirection.Input, idParent);
                dyParam.Add("p_id_nd", OracleDbType.Int64, ParameterDirection.Input, accountId);
                dyParam.Add("rsout", OracleDbType.RefCursor, ParameterDirection.Output);
                return await this.DbConnection.QueryAsync<Department>(
                    QueryResource.Department_GetDepartments,
                    param: dyParam,
                    commandType: System.Data.CommandType.StoredProcedure);
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
        /// Get department by employee id
        /// </summary>
        /// <param name="employeeId">employee id</param>
        /// <returns>The department</returns>
        public async Task<Department> GetDepartmentByEmployeeId(int employeeId)
        {
            var query = @"select dvi.* from don_vi dvi
                join nhan_vien nv on nv.id_donvi = dvi.id_donvi
                where id_nhanvien = :employeeId and dvi.tinh_trang = 1";
            return await this.DbConnection.QueryFirstAsync<Department>(
                query, 
                new { employeeId }, 
                commandType: System.Data.CommandType.Text);
        }

        /// <summary>
        /// Updates a Department
        /// </summary>
        /// <param name="Department">The Department</param>
        /// <returns>the task</returns>
        public async Task UpdateDepartmentAsync(Department department)
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
                dyParam.Add("p_ghi_chu", OracleDbType.NVarchar2, ParameterDirection.Input, department.Ghi_Chu);
                await this.DbConnection.QueryAsync(query, param: dyParam, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get departments with condition: to idDepartmentStart from idDepartmentEnd.
        /// </summary>
        /// <param name="idDepartment">id department start</param>
        /// <param name="accountId">Account Id whose user is performing this action</param>
        /// <param name="idDepartmentEnd">id department End, if idDepartmentEnd = -1 then return all.</param>
        /// <returns>list departments </returns>
        public async Task<IEnumerable<Department>> GetDepartmentsWithTerm(int accountId, int idDepartment, int idDepartmentEnd = -1)
        {
            try
            {
                var dyParam = new OracleDynamicParameters();
                dyParam.Add("p_id_donvi", OracleDbType.Int64, ParameterDirection.Input, idDepartment);
                dyParam.Add("p_end", OracleDbType.Int64, ParameterDirection.Input, idDepartmentEnd);
                dyParam.Add("p_id_nd", OracleDbType.Int64, ParameterDirection.Input, accountId);
                dyParam.Add("RSOUT", OracleDbType.RefCursor, ParameterDirection.Output);
                var query = QueryResource.Department_GetDepartmentsWithTerm;
                return await this.DbConnection.QueryAsync<Department>(
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

        public async Task UpdateEmail(Department department)
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
                await this.DbConnection.QueryAsync(query, param: dyParam, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Gets id departments that managed by the account
        /// </summary>
        /// <param name="accountId">Account id</param>
        /// <returns>List department</returns>
        public async Task<IEnumerable<Department>> GetControlledDepartments(int accountId)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("p_id_nd", OracleDbType.Int64, ParameterDirection.Input, accountId);
            dyParam.Add("rsout", OracleDbType.RefCursor, ParameterDirection.Output);
            return await this.DbConnection.QueryAsync<Department>(
                QueryResource.Department_GetControlledDepartments,
                param: dyParam,
                commandType: System.Data.CommandType.StoredProcedure);
        }
    }
}

