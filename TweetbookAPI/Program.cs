using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TweetbookAPI.Data;
using TweetbookAPI.Services;

namespace TweetbookAPI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            //If you want something to be executed when the application starts, here follows an example of how you can do this
            using (var scope = host.Services.CreateScope())
            {
                var dataContext = scope.ServiceProvider.GetRequiredService<DataContext>();

                //Runs pending migrations, if any
                await dataContext.Database.MigrateAsync();

                //Creates some basic roles for the app
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!await roleManager.RoleExistsAsync(ApplicationRoles.Administrators))
                    await roleManager.CreateAsync(new IdentityRole() { Name = ApplicationRoles.Administrators });

                if (!await roleManager.RoleExistsAsync(ApplicationRoles.Publishers))
                    await roleManager.CreateAsync(new IdentityRole() { Name = ApplicationRoles.Publishers });
            }
                
            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging(loggingConfig =>
                {
                    loggingConfig.ClearProviders();
                    loggingConfig.AddConsole();
                    loggingConfig.AddDebug();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
