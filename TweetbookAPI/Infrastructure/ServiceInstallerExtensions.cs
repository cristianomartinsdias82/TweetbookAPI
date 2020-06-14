using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TweetbookAPI.Infrastructure.ServiceInstallers;
using System;
using System.Linq;

namespace TweetbookAPI.Infrastructure
{
    public static class ServiceInstallerExtensions
    {
        public static void InstallCurrentAssemblyServices(this IServiceCollection services, IConfiguration configuration)
        {
            //Get all concrete implementations of the IInstaller interface in this very assembly, then
            //create instances of these classes, then
            //cast these instances to IInstaller interface, then
            //run InstallServices in every each one of these instances
            typeof(Startup).Assembly.ExportedTypes
                .Where(it => typeof(IInstaller).IsAssignableFrom(it) && !it.IsInterface && !it.IsAbstract)
                .Select(Activator.CreateInstance)
                .Cast<IInstaller>().OrderBy(inst => inst.Order).ToList()
                .ForEach(it => it.InstallServices(services, configuration));
        }
    }
}