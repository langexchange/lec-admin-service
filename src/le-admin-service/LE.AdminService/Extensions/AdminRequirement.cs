using LE.AdminService.Constants;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LE.AdminService.Extensions
{
    public class AdminRequirement : IAuthorizationRequirement
    {
        public string RoleName { get; }

        public AdminRequirement(string roleName)
        {
            RoleName = roleName ?? throw new ArgumentNullException(nameof(roleName));
        }
    }

    public class AdminRequirementHandler : AuthorizationHandler<AdminRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AdminRequirement requirement)
        {
            if (context.User.HasClaim(c => c.Type == ClaimConstant.TYPE && c.Value == requirement.RoleName))
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
