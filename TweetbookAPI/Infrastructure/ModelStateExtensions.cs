using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq;

namespace TweetbookAPI.Infrastructure
{
    public static class ModelStateExtensions
    {
        public static RuleViolationCollection ToRuleViolationList(this ModelStateDictionary modelState)
        {
            if (modelState.IsValid)
                return new RuleViolationCollection();
            else
            {
                var ruleViolations = new RuleViolationCollection();
                var modelStateErrors = modelState
                    .Where(x => x.Value.Errors.Any())
                    .ToDictionary(key => key.Key, value => value.Value.Errors.Select(x => x.ErrorMessage)).ToList();

                modelStateErrors.ForEach(modelStateError =>
                {
                    ruleViolations.AddRange(modelStateError.Value.Select(error =>
                    new RuleViolation()
                    {
                        Field = modelStateError.Key,
                        ErrorMessage = error
                    }));
                });

                return ruleViolations;
            }
        }
    }
}
