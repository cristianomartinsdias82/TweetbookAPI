using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using TweetbookAPI.Infrastructure.Filters;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace TweetbookAPI.Infrastructure.ServiceInstallers
{
    public class SwaggerInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            //MAKE JWT PARAMETERS AVAILABLE THROUGHOUT THE APP
            var apiConfigParameters = new ApiConfigParameters();
            configuration.Bind(nameof(ApiConfigParameters), apiConfigParameters);
            services.AddSingleton(apiConfigParameters);

            //SWAGGER
            services.AddSwaggerGen(options => {
                options.SwaggerDoc(apiConfigParameters.CurrentVersion, new OpenApiInfo() { Title = "Tweetbook API", Version = apiConfigParameters.CurrentVersion });
                options.ExampleFilters(); //For incremented documentation purposes (code continues down below in AddSwaggerExamplesFromAssemblyOf part)
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                //Instructs Swagger to require a new input parameter as per ClientApiKeyValidationOperationFilter class
                //https://stackoverflow.com/questions/41493130/web-api-how-to-add-a-header-parameter-for-all-api-in-swagger
                options.OperationFilter<ClientApiKeyValidationOperationFilter>();

                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme()
                        {
                            Reference = new OpenApiReference()
                            {
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme
                            }
                        }, new List<string>()
                    }
                });

                //The lines below allow to take controller classes xml comments to Swagger documentation. The more documented, the better!
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
            });

            //For Swagger documentation increment purposes
            services.AddSwaggerExamplesFromAssemblyOf<Startup>();
        }
    }
}
