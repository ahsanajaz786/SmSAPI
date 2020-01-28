using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace WebApiJwt.Configurations.Policies
{
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly RedisService _redisService;
        public PermissionHandler([FromServices]RedisService redisService) : base()
        {
            this._redisService = redisService;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            if (!context.User.HasClaim(c => c.Type == "DateOfJoining"))
            {
                return Task.FromResult(0);
            }

            var dateOfJoining = Convert.ToDateTime(context.User.FindFirst(
                c => c.Type == "DateOfJoining").Value);

            double calculatedTimeSpend = (DateTime.Now.Date - dateOfJoining.Date).TotalDays;

            if (calculatedTimeSpend >= requirement.TimeSpendInDays)
            {
                context.Succeed(requirement);
            }
            return Task.FromResult(0);
        }
    }
}
