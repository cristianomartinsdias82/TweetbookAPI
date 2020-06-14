using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using TweetbookAPI.ConfigOptions;
using TweetbookAPI.Infrastructure.SecurityConfiguration;
using TweetbookAPI.Services;
using System.Text;

namespace TweetbookAPI.Infrastructure.ServiceInstallers
{
    public class SecurityConcernsInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            //JWT Parameters
            var jwtConfigOptions = new JwtConfigOptions();
            configuration.Bind(nameof(JwtConfigOptions), jwtConfigOptions);
            services.AddSingleton(jwtConfigOptions); //Make it accessible wherever its requested

            var tokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtConfigOptions.Secret)),
                ValidateIssuer = false,
                ValidateAudience = false,
                RequireExpirationTime = true,
                ValidateLifetime = true
            };

            services.AddSingleton(tokenValidationParameters);

            //AUTHENTICATION
            services.AddAuthentication(options =>
            {
                options.DefaultScheme =
                options.DefaultChallengeScheme =
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            //JWT
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = tokenValidationParameters;
            });

            //AUTHORIZATION POLICIES
            services.AddAuthorization(
                options =>
                {
                    //Claims-based policy - only authenticated users containing the "tag.view" claim in user token is allowed to access TagsController.Get() GET method
                    options.AddPolicy(ResourcePolicies.ViewTagPermissionPolicy, policy => policy.RequireClaim(ResourcePolicies.TryGetPolicyMetaname(ResourcePolicies.ViewTagPermissionPolicy)));

                    //Role-based policy
                    //With the line of code below, you can configure some Role-based policy using [Authorize(Policy=ResourcePolicies.ViewTagPermissionPolicy)] or you can just keep using [Authorize(Roles="a,b...n")] statements, both in controller or action method level :) It is up to you
                    //options.AddPolicy(ResourcePolicies.ViewTagPermissionPolicy, policy => policy.RequireRole(ResourcePolicies.TryGetPolicyMetaname(ResourcePolicies.DeletePostPermissionPolicy)));

                    //Requirement-based (custom logic for a more dynamic approach) policy
                    options.AddPolicy(ResourcePolicies.InternalEmployeePermissionPolicy, policy =>
                    {
                        policy.AddRequirements(new UserWorksForCompanyRequirement("MYCOMPANY.COM.BR"));
                    });

                    //Requirement-based (information ownership) policy
                    options.AddPolicy(ResourcePolicies.InformationOwnershipPermissionPolicy, policy =>
                    {
                        policy.AddRequirements(new InformationOwnershipRequirement());
                    });
                });
            services.AddSingleton<IAuthorizationHandler, UserWorksForCompanyAuthorizationHandler>();
            services.AddSingleton<IAuthorizationHandler, UserIsInformationOwnerAuthorizationHandler>();

            //AUTHENTICATION SERVICE DEPENDENCY
            services.AddScoped<IAuthService, AuthService>();
        }

        public int Order { get; } = 2;
    }
}
