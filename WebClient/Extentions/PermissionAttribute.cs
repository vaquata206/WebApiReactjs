using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace WebClient.Extensions
{
    /// <summary>
    /// Permission attribute
    /// </summary>
    public class PermissionAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// A constructor without param.
        /// </summary>
        public PermissionAttribute()
        {
            this.Policy = PermissionRequirement.PermissionPolicies.Permission;
        }

        /// <summary>
        /// A constructor
        /// </summary>
        /// <param name="isModeUri">Is mode uri</param>
        public PermissionAttribute(bool isModeUri)
        {
            this.Policy = isModeUri ? PermissionRequirement.PermissionPolicies.PermissionUri : PermissionRequirement.PermissionPolicies.Permission;
        }

        /// <summary>
        /// A constructor
        /// </summary>
        /// <param name="linkedController">We will check permission of "linkedController" controller instead of the current action</param>
        /// <param name="linkedAction">We will check permission of "linkedAction" action instead of the current action</param>
        public PermissionAttribute(string linkedController, string linkedAction)
        {
            var path = linkedController + "/" + ("INDEX".Equals(linkedAction.ToUpper()) ? string.Empty : linkedAction);
            this.Policy = !string.IsNullOrEmpty(linkedAction) ? PermissionRequirement.PermissionPolicies.PermissionLinked + path.ToUpper() : PermissionRequirement.PermissionPolicies.Permission;
        }
    }
}
