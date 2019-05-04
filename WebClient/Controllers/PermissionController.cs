using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebClient.Core.Requests;
using WebClient.Extentions;
using WebClient.Services.Interfaces;

namespace WebClient.Controllers
{
    /// <summary>
    /// Permission controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PermissionController : Controller
    {
        /// <summary>
        /// Auth helper
        /// </summary>
        private AuthHelper authHelper;

        /// <summary>
        /// Permission service
        /// </summary>
        private IPermissionService permissionService;

        /// <summary>
        /// Employee permission service
        /// </summary>
        private IEmployeePermissionService employeePermissionService;

        /// <summary>
        /// A constructor
        /// </summary>
        /// <param name="permissionService">Permission service</param>
        /// <param name="employeePermissionService">Employee permission service</param>
        /// <param name="authHelper">Auth helper</param>
        public PermissionController(
            IPermissionService permissionService, 
            IEmployeePermissionService employeePermissionService, 
            AuthHelper authHelper)
        {
            this.permissionService = permissionService;
            this.employeePermissionService = employeePermissionService;
            this.authHelper = authHelper;
        }

        /// <summary>
        /// Get all permission
        /// </summary>
        /// <param name="id">App id</param>
        /// <returns>List permission</returns>
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAll(int id)
        {
            try
            {
                var permissions = await this.permissionService.GetPermissions(id);
                return this.Ok(permissions);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get a permission by Id
        /// </summary>
        /// <param name="id">Permission id</param>
        /// <returns>A permission</returns>
        [HttpGet("[action]")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var permission = await this.permissionService.GetPermissionByIdAsync(id);
                return this.Ok(permission);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get permission of user
        /// </summary>
        /// <param name="userId">Account Id</param>
        /// <param name="appId">App id</param>
        /// <returns>List ids</returns>
        [HttpGet("[action]")]
        public async Task<IActionResult> GetUserPermissions(int userId, int appId)
        {
            try
            {
                var ids = await this.employeePermissionService.GetIdPermissionsOfUser(userId, appId);
                return this.Ok(ids);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Delete a permission
        /// </summary>
        /// <param name="id">permission id</param>
        /// <returns>Action result</returns>
        [HttpDelete("[action]")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await this.permissionService.DeleteAsync(id);
                return this.Ok();
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Delete a permission
        /// </summary>
        /// <param name="permissionRequest">Permission request</param>
        /// <returns>A Action result</returns>
        [HttpPost("[action]")]
        public async Task<IActionResult> Save(PermissionRequest permissionRequest)
        {
            try
            {
                await this.permissionService.SavePermissionAsync(permissionRequest, this.authHelper.UserId);
                return this.Ok();
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
    }
}