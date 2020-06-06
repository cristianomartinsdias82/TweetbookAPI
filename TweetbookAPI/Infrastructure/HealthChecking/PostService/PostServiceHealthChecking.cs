using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TweetbookAPI.Infrastructure.HealthChecking.PostService
{
    /// <summary>
    /// Post service custom health check class
    /// </summary>
    public class PostServiceHealthChecking : IHealthCheck
    {
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            //Custom health check login for Post service here...

            var registration = context.Registration;
            registration.Name = "Post service";

            return await Task.FromResult(HealthCheckResult.Healthy());
        }
    }

    public static class CustomHealthChecksExtensions
    {
        public static IHealthChecksBuilder AddPostServiceCheck(this IHealthChecksBuilder checksBuilder, string registrationName = "Post service")
        {
            return checksBuilder.AddCheck<PostServiceHealthChecking>(registrationName);
        }
    }
}
