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

        /// <summary>
        /// Gets child nodes by parent id. Include: Departments/employees
        /// </summary>
        /// <param name="id">Parent id</param>
        /// <returns>A list node</returns>
        [HttpGet("[action]")]
        public async Task<IActionResult> GetChildNodes(int id)
        {
            try
            {
                var nodes = await this.departmentService.GetChildNodes(id, this.authHelper.UserId);
                return this.Ok(nodes);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get departments by parent. For
        /// </summary>
        /// <param name="id">Parent Id</param>
        /// <returns>List Department</returns>
        [HttpGet("[action]")]
        public async Task<IActionResult> GetDepartmentsByParent(int id)
        {
            try
            {
                var nodes = await this.departmentService.GetDepartmentsByParent(id, this.authHelper.UserId);
                return this.Ok(nodes);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get a department by Id
        /// </summary>
        /// <param name="id">Department id</param>
        /// <returns>A DepartmentVM instance</returns>
        [HttpGet("[action]")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var department = await this.departmentService.GetById(id, this.authHelper.UserId);
                return this.Ok(department);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get All department with format for select item
        /// </summary>
        /// <returns>List department</returns>
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllSelectItems()
        {
            try
            {
                var list = await this.departmentService.GetSelectItems(this.authHelper.UserId);
                return this.Ok(list);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
    }
}