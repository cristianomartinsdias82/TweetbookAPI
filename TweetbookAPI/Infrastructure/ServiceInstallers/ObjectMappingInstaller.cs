using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TweetbookAPI.Infrastructure.ServiceInstallers
{
    public class ObjectMappingInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(typeof(Startup));
        }
    }
}
