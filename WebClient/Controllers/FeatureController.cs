using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebClient.Core.Requests;
using WebClient.Extentions;
using WebClient.Services.Interfaces;

namespace WebClient.Controllers
{
    /// <summary>
    /// Feature controller
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FeatureController : Controller
    {
        /// <summary>
        /// Feature service
        /// </summary>
        private IFeatureService featureService;

        /// <summary>
        /// Auth helper
        /// </summary>
        private AuthHelper authHelper;

        /// <summary>
        /// A constructor of feature controller
        /// </summary>
        /// <param name="authHelper">Auth helper</param>
        /// <param name="featureService">Feature service interface</param>
        public FeatureController(AuthHelper authHelper, IFeatureService featureService)
        {
            this.featureService = featureService;
            this.authHelper = authHelper;
        }

        /// <summary>
        /// Get list menu
        /// </summary>
        /// <returns>List menu</returns>
        [HttpGet("[action]")]
        public async Task<IActionResult> GetMenu()
        {
            var features = await this.featureService.GetFeaturesUser(this.authHelper.UserId);
            return this.Ok(this.featureService.TreeFeaturesToMenu(features));
        }

        /// <summary>
        /// Get all feature
        /// </summary>
        /// <param name="id">App Id</param>
        /// <returns>List features</returns>
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllNodes(int id)
        {
            try
            {
                var features = await this.featureService.GetFeatureNodes(id);
                return this.Ok(features);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Gets feature by Id
        /// </summary>
        /// <param name="id">Feature id</param>
        /// <returns>Feature instance</returns>
        [HttpGet("[action]")]
        public async Task<IActionResult> GetFeature(int id)
        {
            try
            {
                var feature = await this.featureService.GetFeature(id);
                return this.Ok(feature);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
    }
}