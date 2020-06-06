using TweetbookAPI.Contracts.V1.Responses;
using Swashbuckle.AspNetCore.Filters;
using System;

namespace TweetbookAPI.Controllers.V1.SwaggerExamplesDocumentation.Requests
{
    public class MaintainPostResponseExampleDocumentation : IExamplesProvider<MaintainPostResponse>
    {
        public MaintainPostResponse GetExamples()
        {
            return new MaintainPostResponse()
            {
                Id = 1,
                CreationDate = DateTime.Now,
                Author = "Author for XPTO post",
                Title = "XPTO",
                Content = "Content for XPTO post",
                DisplayUntil = DateTime.Now.AddDays(3)
            };
        }
    }
}
