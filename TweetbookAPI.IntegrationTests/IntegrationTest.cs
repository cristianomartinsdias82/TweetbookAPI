using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TweetbookAPI.Contracts.V1.Requests;
using TweetbookAPI.Contracts.V1.Responses;
using TweetbookAPI.Data;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace TweetbookAPI.IntegrationTests
{
    public class IntegrationTest : IDisposable
    {
        protected readonly HttpClient TestClient;
        //private IServiceProvider _serviceProvider;

        public IntegrationTest()
        {
            var appFactory = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        services.RemoveAll(typeof(DataContext));
                        services.AddDbContext<DataContext>(options => options.UseInMemoryDatabase("integration_test_db"));
                    });
                });
            TestClient = appFactory.CreateClient();
            //_serviceProvider = appFactory.Server.Services;
        }

        protected virtual async Task SetupAuthenticationRequestAsync()
        {
            TestClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", await GetAuthJwtAsync());
        }

        private async Task<string> GetAuthJwtAsync()
        {
            //https://stackoverflow.com/questions/19158378/httpclient-not-supporting-postasjsonasync-method-c-sharp
            var response = await TestClient.PostAsJsonAsync("api/v1/signin", new RegisterUserRequest() { UsernameOrEmail = "crisdi@gmail.com", Password = "@Master_File$0" });
            var registrationResponse = await response.Content.ReadAsAsync<RegisterUserResponse>();

            return registrationResponse.Token;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    //Uncomment the lines of code below if you want the database to be completely deleted after each test run
                    //using var _serviceScope = _serviceProvider.CreateScope();
                    //var dataContext = _serviceScope.ServiceProvider.GetService<DataContext>();
                    //dataContext.Database.EnsureDeleted();
                }

                disposedValue = true;
            }
        }

        ~IntegrationTest()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
