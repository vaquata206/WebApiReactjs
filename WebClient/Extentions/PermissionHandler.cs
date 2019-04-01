using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using WebClient.Extentions;
using WebClient.Services.Interfaces;

namespace WebClient.Extensions
{
    /// <summary>
    /// Permission handler
    /// </summary>
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        /// <summary>
        /// Account service
        /// </summary>
        private AuthHelper auth;

        /// <summary>
        /// Feature service
        /// </summary>
        private IFeatureService featureService;

        /// <summary>
        /// A constructor
        /// </summary>
        /// <param name="auth">Account service</param>
        /// <param name="featureService">Feature service</param>
        public PermissionHandler(AuthHelper auth, IFeatureService featureService)
        {
            this.auth = auth;
            this.featureService = featureService;
        }

        /// <summary>
        /// Handler requirement
        /// </summary>
        /// <param name="context">Authorization handler context</param>
        /// <param name="requirement">Permission requirement</param>
        /// <returns>A completed task is returned</returns>
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            // this.accountService.CheckUserPermission()
            try
            {
                var filterContext = (AuthorizationFilterContext)context.Resource;

                var path = string.Empty;
                if (string.IsNullOrEmpty(requirement.LinkedPath))
                {
                    // Get the path of current request
                    path = requirement.IsModelUri ?
                        filterContext.HttpContext.Request.Path.ToString() :
                        filterContext.RouteData.Values["controller"] + "/" + ((filterContext.RouteData.Values["action"].ToString().ToUpper() == "INDEX") ? string.Empty : filterContext.RouteData.Values["action"]);
                }
                else
                {
                    path = requirement.LinkedPath;
                }

                var userId = this.auth.UserId; 

                if (this.featureService.IsAccessedToTheFeature(userId, path, requirement.IsModelUri).Result)
                {
                    context.Succeed(requirement);
                }
            }
            catch (Exception ex)
            {
            }

            return Task.CompletedTask;
        }
    }
}
