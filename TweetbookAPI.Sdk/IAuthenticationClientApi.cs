using TweetbookAPI.Contracts.V1.Requests;
using TweetbookAPI.Contracts.V1.Responses;
using Refit;
using System.Threading.Tasks;

namespace TweetbookAPI.Sdk
{
    [Headers("X-Api-Key: 45buyng498hkunrjhtowe8")]
    public interface IAuthenticationClientApi
    {
        [Post("/api/v1/register")]
        Task<ApiResponse<RegisterUserResponse>> Register([Body] RegisterUserRequest request);

        [Post("/api/v1/signin")]
        Task<ApiResponse<AuthenticateUserResponse>> SignIn([Body] AuthenticateUserRequest request);

        [Post("/api/v1/refresh")]
        Task<ApiResponse<AuthenticateUserResponse>> Refresh([Body] RefreshTokenRequest request);
    }
}
