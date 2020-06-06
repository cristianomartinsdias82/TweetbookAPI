using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TweetbookAPI.Infrastructure.Filters
{
    public class ClientApiKeyValidationFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var isValid = false;
            var informedApiKey = context.HttpContext.Request.Headers["X-Api-Key"];
            if (!string.IsNullOrEmpty(informedApiKey))
            {
                //further apikey validation custom logic here...
                //var appConfig = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
                //var appKey = appConfig.GetValue<string>("ApiConfigParameters:Key");
                var appKey = context.HttpContext.RequestServices.GetRequiredService<ApiConfigParameters>().Key;

                //example:
                isValid = informedApiKey.Equals(appKey);

                //...

                //isValid = true;
            }

            if (isValid)
                await next();
            else
                context.Result = new UnauthorizedObjectResult(new { error = "You must provide a valid api key" });
        }
    }

    public class ClientApiKeyValidationOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();

            operation.Parameters.Add(new OpenApiParameter()
            {
                Name = "X-Api-Key",
                In = ParameterLocation.Header,
                Required = true
            });
        }
    }
}
