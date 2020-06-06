using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TweetbookAPI.Data;
using TweetbookAPI.Infrastructure.HealthChecking.PostService;
using TweetbookAPI.Services;

namespace TweetbookAPI.Infrastructure.ServiceInstallers
{
    public class HealthChecksInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddHealthChecks()
            .AddDbContextCheck<DataContext>()
            .AddPostServiceCheck(nameof(PostService)) //Custom extension method
            .AddCheck<PostServiceHealthChecking>("Post service");
        }
    }
}
