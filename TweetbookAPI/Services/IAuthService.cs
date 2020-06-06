using TweetbookAPI.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TweetbookAPI.Services
{
    public interface IAuthService : IDisposable
    {
        Task<AuthenticationResult> RegisterAsync(string usernameOrEmail, string password);
        Task<AuthenticationResult> AuthenticateAsync(string usernameOrEmail, string password);
        Task<AuthenticationResult> RefreshTokenAsync(string token, string refreshToken);
    }
}
