using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<ActionResult> Save(EmployeeVM employeeVM)
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
        public async Task<ActionResult> Delete(string code)
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
    }
}
