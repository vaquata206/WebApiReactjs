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
    public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository
    {
        /// <summary>
        /// Update information's Employee
        /// </summary>
        /// <param name="employee">the employee</param>
        /// <returns>the employee after updated</returns>
        public async Task<Employee> UpdateInformationEmployee(Employee employee, int currUser)
        {
            try
            {
                var dyParam = new OracleDynamicParameters();
                dyParam.Add("p_id_nv_capnhat", OracleDbType.Int64, ParameterDirection.Input, currUser);
                dyParam.Add("p_id_nhanvien", OracleDbType.Varchar2, ParameterDirection.Input, employee.Id_NhanVien);
                dyParam.Add("p_ho_ten", OracleDbType.Varchar2, ParameterDirection.Input, employee.Ho_Ten);
                dyParam.Add("p_dia_chi", OracleDbType.Varchar2, ParameterDirection.Input, employee.Dia_Chi);
                dyParam.Add("p_dien_thoai", OracleDbType.Varchar2, ParameterDirection.Input, employee.Dien_Thoai);
                dyParam.Add("p_email", OracleDbType.Varchar2, ParameterDirection.Input, employee.Email);
                dyParam.Add("p_nam_sinh", OracleDbType.Date, ParameterDirection.Input, employee.Nam_Sinh);
                dyParam.Add("p_so_cmnd", OracleDbType.Varchar2, ParameterDirection.Input, employee.So_CMND);
                dyParam.Add("p_ngaycap_cmnd", OracleDbType.Date, ParameterDirection.Input, employee.NgayCap_CMND);
                dyParam.Add("p_noicap_cmnd", OracleDbType.Varchar2, ParameterDirection.Input, employee.NoiCap_CMND);
                dyParam.Add("p_ghi_chu", OracleDbType.Varchar2, ParameterDirection.Input, employee.Ghi_Chu);
                dyParam.Add("p_chuc_vu", OracleDbType.Int64, ParameterDirection.Input, employee.Chuc_Vu);
                dyParam.Add("RSOUT", OracleDbType.RefCursor, ParameterDirection.Output);
                var query = QueryResource.Employee_Update;
                return await this.DbConnection.QueryFirstOrDefaultAsync<Employee>(query, param: dyParam, commandType: CommandType.StoredProcedure, commandTimeout: 5000);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get employee by idNhanVien
        /// </summary>
        /// <param name="idNhanVien"></param>
        /// <returns>the employee</returns>
        public async Task<Employee> GetEmployeeById(int idNhanVien)
        {
            try
            {
                var dyParam = new OracleDynamicParameters();
                dyParam.Add("p_id_nhanvien", OracleDbType.Varchar2, ParameterDirection.Input, idNhanVien);
                dyParam.Add("RSOUT", OracleDbType.RefCursor, ParameterDirection.Output);
                var query = QueryResource.Employee_Get_By_Id;
                return await this.DbConnection.QueryFirstOrDefaultAsync<Employee>(query, param: dyParam, commandType: CommandType.StoredProcedure, commandTimeout: 5000);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get employee by user code
        /// </summary>
        /// <param name="userCode">User code</param>
        /// <returns>A Employee</returns>
        public async Task<Employee> GetEmployeeByCode(string userCode)
        {
            var query = @"select * from nhan_vien where ma_nhanvien = :userCode";
            return await this.DbConnection.QueryFirstOrDefaultAsync<Employee>(
                query,
                param: new { userCode = userCode },
                commandType: CommandType.Text);
        }

        /// <summary>
        /// Get employees by department id
        /// </summary>
        /// <param name="deparmentId">Id of deparment</param>
        /// <returns>List employee</returns>
        public async Task<IEnumerable<Employee>> GetEmployeesByDeparmentId(int deparmentId)
        {
            var query = @"Select * FROM Nhan_Vien Where Id_DonVi = :deparmentId and Tinh_Trang = 1";
            return await this.DbConnection.QueryAsync<Employee>(query, param: new { deparmentId = deparmentId }, commandType: System.Data.CommandType.Text);
        }

        /// <summary>
        /// get all employees
        /// </summary>
        /// <returns>lst employees</returns>
        public async Task<IEnumerable<Employee>> GetAllEmployess()
        {
            try
            {
                var query = QueryResource.Employee_Get_ALL;
                var dyParam = new OracleDynamicParameters();
                dyParam.Add("RSOUT", OracleDbType.RefCursor, ParameterDirection.Output);
                var obj = await this.DbConnection.QueryAsync<Employee>(query, param: dyParam, commandType: CommandType.StoredProcedure, commandTimeout: 5000);
                return obj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// delete employee
        /// </summary>
        /// <param name="employee">Employee instance</param>
        /// <param name="handler">Handler id</param>
        /// <returns>the task</returns>
        public async Task<Employee> DeteteEmployee(Employee employee, int handler)
        {
            try
            {
                var query = QueryResource.Employee_DeleteById;
                var dyParam = new OracleDynamicParameters();
                dyParam.Add("p_id_nhanvien", OracleDbType.Int64, ParameterDirection.Input, employee.Id_NhanVien);
                dyParam.Add("p_id_nv_capnhat", OracleDbType.Int64, ParameterDirection.Input, handler);
                dyParam.Add("rsout", OracleDbType.RefCursor, ParameterDirection.Output);

                return await this.DbConnection.QueryFirstOrDefaultAsync<Employee>(
                    query, 
                    param: dyParam, 
                    commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// insert new employee
        /// </summary>
        /// <param name="employee">the new employee</param>
        /// <param name="curUser">the current idnhanvien</param>
        /// <returns>the task</returns>
        public async Task<Employee> InsertEmployee(Employee employee,int curUser)
        {
            try
            {
                var query = QueryResource.Employee_Insert;
                var dyParam = new OracleDynamicParameters();

                dyParam.Add("p_ma_nhanvien", OracleDbType.Varchar2, ParameterDirection.Input, employee.Ma_NhanVien);
                dyParam.Add("p_ho_ten", OracleDbType.Varchar2, ParameterDirection.Input, employee.Ho_Ten);
                dyParam.Add("p_dia_chi", OracleDbType.Varchar2, ParameterDirection.Input, employee.Dia_Chi);
                dyParam.Add("p_dien_thoai", OracleDbType.Varchar2, ParameterDirection.Input, employee.Dien_Thoai);
                dyParam.Add("p_email", OracleDbType.Varchar2, ParameterDirection.Input, employee.Email);     
                dyParam.Add("p_nam_sinh", OracleDbType.Date, ParameterDirection.Input, employee.Nam_Sinh);
                dyParam.Add("p_so_cmnd", OracleDbType.Varchar2, ParameterDirection.Input, employee.So_CMND);
                dyParam.Add("p_ngaycap_cmnd", OracleDbType.Date, ParameterDirection.Input, employee.NgayCap_CMND);
                dyParam.Add("p_noicap_cmnd", OracleDbType.Varchar2, ParameterDirection.Input, employee.NoiCap_CMND);
                dyParam.Add("p_id_donvi", OracleDbType.Int64, ParameterDirection.Input, employee.Id_DonVi);
                dyParam.Add("p_chuc_vu", OracleDbType.Int64, ParameterDirection.Input, employee.Chuc_Vu);
                dyParam.Add("p_id_nv_khoitao", OracleDbType.Int64, ParameterDirection.Input, curUser);
                dyParam.Add("p_ngay_khoitao", OracleDbType.Date, ParameterDirection.Input, DateTime.Now);
                dyParam.Add("p_ghi_chu", OracleDbType.Varchar2, ParameterDirection.Input, employee.Ghi_Chu);
                dyParam.Add("rsout", OracleDbType.RefCursor, ParameterDirection.Output);

                return await this.DbConnection.QueryFirstOrDefaultAsync<Employee>(query, param: dyParam, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
