using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebClient.Core.Responses;
using WebClient.Core.ViewModels;
using WebClient.Extensions;
using WebClient.Extentions;
using WebClient.Services.Interfaces;

namespace WebClient.Controllers
{
    /// <summary>
    /// Employee controller
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : Controller
    {
        /// <summary>
        /// The employee service
        /// </summary>
        private IEmployeeService employeeService;

        /// <summary>
        /// the auth helper
        /// </summary>
        private AuthHelper authHelper;

        /// <summary>
        /// Employee controller
        /// </summary>
        /// <param name="employeeService">Employee service</param>
        /// <param name="auth">Auth helper</param>
        public EmployeeController(IEmployeeService employeeService, AuthHelper auth)
        {
            this.employeeService = employeeService;
            this.authHelper = auth;
        }

        /// <summary>
        /// save employee
        /// </summary>
        /// <param name="employeeVM">the employeeVM</param>
        /// <returns>the View</returns>
        [Permission("employee", "index")]
        [HttpPost("[action]")]
        public async Task<IActionResult> Save(EmployeeVM employeeVM)
        {
            try
            {
                var employee = await this.employeeService.SaveEmployee(employeeVM, this.authHelper.UserId);
                return this.Ok(employee);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex);
            }
        }

        /// <summary>
        /// Delete employee
        /// </summary>
        /// <param name="code">User code</param>
        /// <returns>Action result</returns>
        [HttpGet("[action]")]
        [Permission("employee", "index")]
        public async Task<IActionResult> Delete(string code)
        {
            try
            {
                var employee = await this.employeeService.DeleteEmployeeByCode(code, this.authHelper.UserId);
                return this.Ok(employee);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex);
            }
        }

        /// <summary>
        /// Get employees by department id
        /// </summary>
        /// <param name="id">department id</param>
        /// <returns>List employee</returns>
        [HttpGet("[action]")]
        public async Task<IActionResult> GetByDepartmentId(int id)
        {
            try
            {
                var employees = await this.employeeService.GetEmployeesByDeparmentId(id);
                return this.Ok(employees.Select(x => new SubEmployeeResponse
                {
                    Ho_Ten = x.Ho_Ten,
                    Id = x.Id_NhanVien,
                    Ma_NhanVien = x.Ma_NhanVien
                }));
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get a employee by employee code
        /// </summary>
        /// <param name="code">Employee Code</param>
        /// <returns>A Employee instance</returns>
        [HttpGet("[action]")]
        public async Task<IActionResult> Get(string code)
        {
            try
            {
                var employee = await this.employeeService.GetEmployeeByCode(code);
                return this.Ok(employee);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
    }
}
