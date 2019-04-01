using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace WebClient.Extensions
{
    /// <summary>
    /// Permission policy provider
    /// </summary>
    public class PermissionPolicyProvider : IAuthorizationPolicyProvider
    {
        /// <summary>
        /// Policy perfix
        /// </summary>
        private const string PolicyPrefix = "PermissionUser";

        /// <summary>
        /// A constructor
        /// </summary>
        /// <param name="options">Authorization options</param>
        public PermissionPolicyProvider(IOptions<AuthorizationOptions> options)
        {
            this.FallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
        }

        /// <summary>
        /// Fall back policy provider
        /// </summary>
        public DefaultAuthorizationPolicyProvider FallbackPolicyProvider { get; }

        /// <summary>
        /// Get default policy async
        /// </summary>
        /// <returns>Authorization policy</returns>
        public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
        {
            return Task.FromResult(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build());
        }

        /// <summary>
        /// Get policy async
        /// </summary>
        /// <param name="policyName">Policy name</param>
        /// <returns>Authorization policy</returns>
        public Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            if (policyName.StartsWith(PolicyPrefix, StringComparison.OrdinalIgnoreCase))
            {
                var policy = new AuthorizationPolicyBuilder();
                
                if (policyName == PermissionRequirement.PermissionPolicies.Permission)
                {
                    policy.AddRequirements(new PermissionRequirement());
                }
                else if (policyName == PermissionRequirement.PermissionPolicies.PermissionUri)
                {
                    policy.AddRequirements(new PermissionRequirement() { IsModelUri = true });
                }
                else if (policyName.StartsWith(PermissionRequirement.PermissionPolicies.PermissionLinked))
                {
                    var path = policyName.Substring(PermissionRequirement.PermissionPolicies.PermissionLinked.Length);
                    policy.AddRequirements(new PermissionRequirement() { LinkedPath = path });
                }

                return Task.FromResult(policy.Build());
            }

            // If the policy name doesn't match the format expected by this policy provider,
            // try the fallback provider. If no fallback provider is used, this would return 
            // Task.FromResult<AuthorizationPolicy>(null) instead.
            return this.GetDefaultPolicyAsync();
        }
    }
}
