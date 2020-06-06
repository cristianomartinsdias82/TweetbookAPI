using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace TweetbookAPI.Infrastructure
{
    public static class HttpContextExtensions
    {
        /// <summary>
        /// Gets the current request context user id
        /// https://stackoverflow.com/questions/30701006/how-to-get-the-current-logged-in-user-id-in-asp-net-core
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetCurrentUserId(this HttpContext context)
        {
            var user = context.User;
            if (user != null && user.Identity != null && user.Identity.IsAuthenticated)
                return user.FindFirstValue("id");

            return null;
        }
    }
}
