using TweetbookAPI.Contracts.V1.Requests;
using Swashbuckle.AspNetCore.Filters;
using System;

namespace TweetbookAPI.Controllers.V1.SwaggerExamplesDocumentation.Requests
{
    public class MaintainPostRequestExampleDocumentation : IExamplesProvider<MaintainPostRequest>
    {
        public MaintainPostRequest GetExamples()
        {
            return new MaintainPostRequest()
            {
                CreationDate = DateTime.Now,
                Author = "Author for XPTO post",
                Title = "XPTO",
                Content = "Content for XPTO post",
                DisplayUntil = DateTime.Now.AddDays(3)
            };
        }
    }
}
