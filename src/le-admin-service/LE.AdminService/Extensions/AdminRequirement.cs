using LE.AdminService.Constants;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LE.AdminService.Extensions
{
    public class AuthRequirement : IAuthorizationRequirement
    {
        public List<string> RoleName { get; }

        public AuthRequirement(List<string> roleName)
        {
            RoleName = roleName ?? throw new ArgumentNullException(nameof(roleName));
        }
    }

    public class AuthRequirementHandler : AuthorizationHandler<AuthRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AuthRequirement requirement)
        {
            if (context.User.HasClaim(c => c.Type == ClaimConstant.TYPE && requirement.RoleName.Contains(c.Value)))
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
