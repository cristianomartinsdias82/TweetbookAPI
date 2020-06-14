using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net;
using System.Threading.Tasks;
using TweetbookAPI.Services;

namespace TweetbookAPI.Infrastructure.Filters
{
    /// <summary>
    /// This validation filter makes use of a registered policy by using a built-in IAuthorizationService interface
    /// which receives the policy name along with the resource object to perform the validation.
    /// (In this moment, the policy's related requirement and handler - registered when SecurityConcernsInstaller is executed - classes get involved)
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class PostOwnershipValidationFilter : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var services = context.HttpContext.RequestServices;
            var authorizationService = services.GetRequiredService<IAuthorizationService>();
            var postService = services.GetRequiredService<IPostService>();
            
            var postId = (int)context.ActionArguments["id"];
            var post = await postService.GetByIdAsync(postId);

            if (post == null) //Allow the request to proceed, leaving the remaining infrastructure code in charge of dealing with this specific (not found) situation
            {
                await next();
                return;
            }

            var result = await authorizationService.AuthorizeAsync(context.HttpContext.User, post, ResourcePolicies.InformationOwnershipPermissionPolicy);

            if (result.Succeeded)
                await next();
            else
                context.Result = new StatusCodeResult((int)HttpStatusCode.Forbidden);
        }
    }
}
