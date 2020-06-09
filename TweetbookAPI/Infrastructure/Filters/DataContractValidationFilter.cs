using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Threading.Tasks;

namespace TweetbookAPI.Infrastructure.Filters
{
    public class DataContractValidationFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //The FluentValidation package had already done its job when the runtime reaches this line.
            var ruleViolations = context.ModelState.ToRuleViolationList();
            if (ruleViolations == null || !ruleViolations.Any())
                await next();
            else
                context.Result = new BadRequestObjectResult(ruleViolations);
        }
    }
}
