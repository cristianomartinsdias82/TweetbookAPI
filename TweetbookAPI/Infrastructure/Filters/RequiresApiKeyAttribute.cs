using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace TweetbookAPI.Infrastructure.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class RequiresApiKeyAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var isValid = false;
            var apiKey = context.HttpContext.Request.Headers["X-Api-Key"];
            if (!string.IsNullOrEmpty(apiKey))
            {
                var apiConfigParameters = context.HttpContext.RequestServices.GetRequiredService<ApiConfigParameters>();

                //further apikey validation custom logic here...

                //example:
                isValid = apiKey.Equals(apiConfigParameters.Key);

                //...

                //isValid = true;
            }

            if (isValid)
                await next();
            else
                context.Result = new UnauthorizedObjectResult(new { error = "You must provide a valid api key" });
        }
    }
}
