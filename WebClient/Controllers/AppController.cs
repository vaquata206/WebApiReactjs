using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebClient.Core.Request;
using WebClient.Extentions;
using WebClient.Services.Interfaces;

namespace WebClient.Controllers
{
    /// <summary>
    /// Application controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AppController : ControllerBase
    {
        /// <summary>
        /// App Service
        /// </summary>
        private IAppService appService;

        /// <summary>
        /// Auth helper
        /// </summary>
        private AuthHelper authHelper;

        /// <summary>
        /// A constructor
        /// </summary>
        /// <param name="appService">App service</param>
        /// <param name="authHelper">Auth helper</param>
        public AppController(IAppService appService, AuthHelper authHelper)
        {
            this.appService = appService;
            this.authHelper = authHelper;
        }

        /// <summary>
        /// Get all apps
        /// </summary>
        /// <returns>List app</returns>
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var list = await this.appService.GetAll();
                return this.Ok(list);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get apps
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns>list app</returns>
        [HttpGet("[action]")]
        public async Task<IActionResult> GetUserApps(int id)
        {
            try
            {
                var apps = await this.appService.GetUserAppsAsync(id <= 0 ? this.authHelper.UserId : id);
                return this.Ok(apps);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Set permission
        /// </summary>
        /// <param name="userApps">User apps</param>
        /// <returns>Action result</returns>
        [HttpPost("[action]")]
        public async Task<IActionResult> SetPermission(UserAppsRequest userApps)
        {
            try
            {
                await this.appService.SetPermission(userApps, this.authHelper.UserId);
                return this.Ok();
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
    }
}