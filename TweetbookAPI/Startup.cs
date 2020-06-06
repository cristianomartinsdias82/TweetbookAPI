using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TweetbookAPI.ConfigOptions;
using TweetbookAPI.Infrastructure.HealthChecking;
using TweetbookAPI.Infrastructure;
using System.Linq;
using System.Net.Mime;

namespace TweetbookAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.InstallCurrentAssemblyServices(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseHsts();

            app.UseHealthChecks("/health", new HealthCheckOptions()
            {
                ResponseWriter = async (context, healthReport) => {

                    context.Response.ContentType = MediaTypeNames.Application.Json;

                    var healthCheckData = new HealthCheckDataResponse()
                    {
                        Status = healthReport.Status.ToString(),
                        Duration = healthReport.TotalDuration,
                        Data = healthReport.Entries.Select(it => new HealthCheckData() { Component = it.Key, Description = it.Value.Description, Status = it.Value.Status.ToString() })
                    };

                    await context.Response.WriteAsync(JsonConvert.SerializeObject(healthCheckData));
                }
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseAuthentication();
            
            var swaggerConfigOptions = new SwaggerConfigOptions();
            var apiConfigParameters = new ApiConfigParameters();
            Configuration.Bind(nameof(SwaggerConfigOptions), swaggerConfigOptions);
            Configuration.Bind(nameof(ApiConfigParameters), apiConfigParameters);
            app.UseSwagger(options =>
            {
                options.RouteTemplate = swaggerConfigOptions.JsonRoute;
            });
            app.UseSwaggerUI(options => {
                options.SwaggerEndpoint(swaggerConfigOptions.UIEndpoint.Replace("{CurrentVersion}", apiConfigParameters.CurrentVersion), swaggerConfigOptions.Description);

                //options.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(options =>
            {
                options.MapDefaultControllerRoute();
            });
        }
    }
}
