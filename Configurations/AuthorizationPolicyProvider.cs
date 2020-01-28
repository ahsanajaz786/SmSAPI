using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IdentityModel;

namespace WebApiJwt.Configurations
{
    public class AuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
    {
        public AuthorizationPolicyProvider(IOptions<AuthorizationOptions> options): base(options)
        {

        }
        public override Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            if (!policyName.StartsWith(PermissionAuthorizeAttribute.PolicyPrefix, StringComparison.OrdinalIgnoreCase))
            {
                return base.GetPolicyAsync(policyName);
            }

            var permissionNames = policyName.Substring(PermissionAuthorizeAttribute.PolicyPrefix.Length).Split(',');

            var policy = new AuthorizationPolicyBuilder()
                .RequireClaim("PERMISSION", permissionNames)
                .Build();

            return Task.FromResult(policy);
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class PermissionAuthorizeAttribute : AuthorizeAttribute
    {
        internal const string PolicyPrefix = "PERMISSION:";/// <summary>
                                                           /// Creates a new instance of <see cref="AuthorizeAttribute"/> class.
                                                           /// </summary>
                                                           /// <param name="permissions">A list of permissions to authorize</param>
        public PermissionAuthorizeAttribute(params PermissionNames[] permissions)
        {
            
            Policy = $"{PolicyPrefix}{string.Join(",", permissions.Select(e => (int)e))}";
        }
    }

    public sealed class PermissionAuthorizationRequirement : AuthorizationHandler<PermissionAuthorizationRequirement>,IAuthorizationRequirement
    {
        public PermissionAuthorizationRequirement(List<PermissionNames> permissions)
        {
            Permissions = permissions;
        }

        public List<PermissionNames> Permissions { get; }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            PermissionAuthorizationRequirement requirement)
        {
            if (context.User == null || requirement.Permissions == null || !requirement.Permissions.Any())
                return Task.CompletedTask;

            //Getting user allocated permissions
            var userPermissions = context.User.Claims.Where(e => e.Type == "Permission").Select(e => e.Value).FirstOrDefault().Split(new char[] { ','},StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();

            //Checking the required permission
            foreach(var e in Permissions.Select(e=>(int)e))
            {
                //If any of the required permission is not present in the system then return the user immediately
                if (!userPermissions.Contains(e))
                    return Task.CompletedTask;
            }
            context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
