using FluentValidation.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TweetbookAPI.Contracts.V1.Requests.Validation;
using TweetbookAPI.Infrastructure.Filters;
using TweetbookAPI.Services;

namespace TweetbookAPI.Infrastructure.ServiceInstallers
{
    public class MvcInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            //MVC WTH ROUTES
            //Api route prefix config. (Please refer to https://stackoverflow.com/questions/58340979/how-to-add-global-route-prefix-in-asp-net-core-3)
            services.AddMvc(
                options =>
                {
                    options.UseGeneralRoutePrefix("api/v{version:apiVersion}");

                    //This filter requires the clients to send a valid api key on a per-request basis
                    options.Filters.Add<ClientApiKeyValidationFilter>(order:1);

                    options.Filters.Add<DataContractValidationFilter>(order:2); //Input parameter validation
                    //This filter aims to centralize action method validation logic, i.e.,
                    //the developer does not need to write data validation logic (ModelState) in each and every action method in order to return BadRequest to the users, for example.
                })
                .AddFluentValidation(options => options.RegisterValidatorsFromAssembly(typeof(MaintainPostRequestValidator).Assembly));

            //API VERSIONING
            services.AddApiVersioning(options => options.ReportApiVersions = true);

            //HTTPCONTEXT SERVICE DEPENDENCY - allows the user to retrieve the current user identity data via HttpContext.GetCurrentUserId() custom extension method
            // https://stackoverflow.com/questions/30701006/how-to-get-the-current-logged-in-user-id-in-asp-net-core
            services.AddHttpContextAccessor();

            //POST SERVICES
            services.AddScoped<IPostService, PostService>();

            //DATETIME ADAPTER
            services.AddTransient<IDateTime, DateTimeAdapter>();
        }

        public int Order { get; } = 1;
    }
}
