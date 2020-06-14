using Microsoft.AspNetCore.Authorization;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using TweetbookAPI.Domain;

namespace TweetbookAPI.Infrastructure.SecurityConfiguration
{
    public class InformationOwnershipRequirement : IAuthorizationRequirement {}

    public class UserIsInformationOwnerAuthorizationHandler : AuthorizationHandler<InformationOwnershipRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, InformationOwnershipRequirement requirement)
        {
            if (CurrentUserIsOwner(context.User, context.Resource as Post))
                context.Succeed(requirement);
            else
                context.Fail();

            return Task.CompletedTask;
        }

        private bool CurrentUserIsOwner(ClaimsPrincipal principal, Post post) =>
            string.Equals(principal.FindFirstValue("id"), post.UserId, StringComparison.Ordinal);
    }
}
