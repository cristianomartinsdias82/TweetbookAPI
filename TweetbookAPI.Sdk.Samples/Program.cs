using TweetbookAPI.Contracts.V1.Requests;
using Refit;
using System;
using System.Threading.Tasks;

namespace TweetbookAPI.Sdk.Samples
{
    class Program
    {
        static async Task Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            const string BASE_API_URL = "https://localhost:5001";
            var authToken = string.Empty;

            //1 - Creates the clients for authentication and tweetbook service endpoints
            var authenticationApiClient = RestService.For<IAuthenticationClientApi>(BASE_API_URL);
            var tweetbookApiClient = RestService.For<ITweetbookClientApi>(BASE_API_URL, new RefitSettings()
            {
                AuthorizationHeaderValueGetter = () => {
                    return Task.FromResult(authToken);
                }
            });
            
            //2 - Invoke service methods
            //2.1 Register method
            var registerResponse = await authenticationApiClient.Register(new RegisterUserRequest()
            {
                UsernameOrEmail = "refit-created-user@apisdk.com",
                Password = "l#_ksDjfg-8*"
            });

            var postRegistrationTokenData = registerResponse.Content;
            Console.WriteLine("(Registration) data", postRegistrationTokenData);

            //2.2 SignIn method
            var signInResponse = await authenticationApiClient.SignIn(new AuthenticateUserRequest()
            {
                UsernameOrEmail = "crisdi@gmail.com",
                Password = "@Master_File$0"
            });

            var postAuthenticationData = signInResponse.Content;
            Console.WriteLine($"(Authentication)  Success: {postAuthenticationData.Success}");
            if (postAuthenticationData.Success)
            {
                authToken = postAuthenticationData.Token; //Stores the token for reuse i TweetBook api client
                Console.WriteLine($"(Authentication) Token: {authToken}");
                Console.WriteLine($"(Authentication) Refresh token: {postAuthenticationData.RefreshToken}");

                //2.3 Create Post method
                var createPostResponse = await tweetbookApiClient.Post(new MaintainPostRequest()
                {
                    Author = "SDK",
                    Content = "Content created dynamically by the Tweetbook SDK",
                    CreationDate = DateTime.Now,
                    DisplayUntil = DateTime.Now.AddDays(7),
                    Title = "The Post title (by SDK)"
                });

                if (createPostResponse.Id > 0)
                {
                    Console.WriteLine($"Author: {createPostResponse.Author}");
                    Console.WriteLine($"Content: {createPostResponse.Content}");
                    Console.WriteLine($"Created date: {createPostResponse.CreationDate}");
                    Console.WriteLine($"Display until: {createPostResponse.DisplayUntil}");
                    Console.WriteLine($"Title: {createPostResponse.Title}");
                }
            }

            await Task.CompletedTask;
        }
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var exc = e.ExceptionObject as Exception;
            Console.WriteLine(exc.Message);
            Console.WriteLine(exc.StackTrace);
        }
    }   
}
