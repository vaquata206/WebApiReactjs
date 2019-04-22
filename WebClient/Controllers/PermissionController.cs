using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebClient.Extentions;
using WebClient.Services.Interfaces;

namespace WebClient.Controllers
{
    /// <summary>
    /// Permission controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
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
        /// A constructor
        /// </summary>
        /// <param name="permissionService">Permission service</param>
        /// <param name="authHelper">Auth helper</param>
        public PermissionController(IPermissionService permissionService, AuthHelper authHelper)
        {
            this.permissionService = permissionService;
            this.authHelper = authHelper;
        }

        /// <summary>
        /// Get all permission
        /// </summary>
        /// <returns>List permission</returns>
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var permissions = await this.permissionService.GetPermissions();
                return this.Ok(permissions);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
    }
}