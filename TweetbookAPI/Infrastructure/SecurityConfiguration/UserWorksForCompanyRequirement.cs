using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TweetbookAPI.Infrastructure.SecurityConfiguration
{
    public class UserWorksForCompanyRequirement : IAuthorizationRequirement
    {
        public string DomainName { get; set; }

        public UserWorksForCompanyRequirement(string domainName)
        {
            DomainName = domainName;
        }
    }

    public class UserWorksForCompanyAuthorizationHandler : AuthorizationHandler<UserWorksForCompanyRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, UserWorksForCompanyRequirement requirement)
        {
            if (!(context.User?.Identity.IsAuthenticated ?? false))
                context.Fail();
            else
            {
                var userEmail = context.User.FindFirstValue(ClaimTypes.Email);

                if (string.IsNullOrEmpty(userEmail))
                    context.Fail();

                if (userEmail.ToUpperInvariant().EndsWith(requirement.DomainName))
                    context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
