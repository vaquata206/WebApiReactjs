using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebClient.Core.ViewModels;
using WebClient.Extentions;
using WebClient.Services.Interfaces;

namespace WebClient.Controllers
{
    /// <summary>
    /// Department controller
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DepartmentController : Controller
    {
        /// <summary>
        /// Department service
        /// </summary>
        private IDepartmentService departmentService;

        /// <summary>
        /// Auth helper
        /// </summary>
        private AuthHelper authHelper;

        /// <summary>
        /// Department controller
        /// </summary>
        /// <param name="departmentService">Department service</param>
        /// <param name="authHelper">Auth helper</param>
        public DepartmentController(IDepartmentService departmentService, AuthHelper authHelper)
        {
            this.departmentService = departmentService;
            this.authHelper = authHelper;
        }

        /// <summary>
        /// Delete the department
        /// </summary>
        /// <param name="code">Department code</param>
        /// <returns>The action</returns>
        [HttpGet("[action]")]
        public async Task<IActionResult> Delete(string code)
        {
            try
            {
                var department = await this.departmentService.DeleteDepartmentAsync(code, this.authHelper.UserId);
                return this.Ok(department);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Save department
        /// </summary>
        /// <param name="departmentVM">Department VM</param>
        /// <returns>Action result</returns>
        [HttpPost("[action]")]
        public async Task<IActionResult> Save(DepartmentVM departmentVM)
        {
            try
            {
                var department = await this.departmentService.SaveDepartment(departmentVM, this.authHelper.UserId);
                return this.Ok(department);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// action update email of department
        /// </summary>
        /// <param name="emailDepartmentVM">the emailDepartment view model</param>
        /// <returns>the view detail </returns>
        [HttpPost("[action]")]
        public async Task<IActionResult> UpdateEmail(EmailDepartmentVM emailDepartmentVM)
        {
            try
            {
                var department = await this.departmentService.UpdateEmail(emailDepartmentVM, this.authHelper.UserId, this.authHelper.DepartmentId);
                return this.Ok(department);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
    }
}