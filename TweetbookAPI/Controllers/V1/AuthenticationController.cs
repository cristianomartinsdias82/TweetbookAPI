using Microsoft.AspNetCore.Mvc;
using TweetbookAPI.Contracts.V1.Requests;
using TweetbookAPI.Contracts.V1.Responses;
using TweetbookAPI.DTO;
using TweetbookAPI.Infrastructure;
using TweetbookAPI.Services;
using System.Threading.Tasks;

namespace TweetbookAPI.Controllers.V1
{
    [ApiVersion("1")]
    [ApiVersion("2")]
    public class AuthenticationController : TweetbookApiController
    {
        private readonly IAuthService _authenticationService;

        public AuthenticationController(IAuthService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [MapToApiVersion("1")]
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserRequest registerUserRequest)
        {
            var registrationResult = await _authenticationService.RegisterAsync(registerUserRequest.UsernameOrEmail, registerUserRequest.Password);

            if (!registrationResult.Success)
                return BadRequest(new AuthenticationResult() { Errors = registrationResult.Errors });

            return Ok(registrationResult.CastTo<RegisterUserResponse>());
        }

        [MapToApiVersion("1")]
        [HttpPost("signin")]
        public async Task<ActionResult> SignIn(AuthenticateUserRequest authenticateUserRequest)
        {
            var authenticationResult = await _authenticationService.AuthenticateAsync(authenticateUserRequest.UsernameOrEmail, authenticateUserRequest.Password);

            if (!authenticationResult.Success)
                return BadRequest(new AuthenticationResult() { Errors = authenticationResult.Errors });

            return Ok(authenticationResult.CastTo<AuthenticateUserResponse>());
        }

        [MapToApiVersion("1")]
        [HttpPost("refresh")]
        public async Task<ActionResult> Refresh(RefreshTokenRequest refreshTokenRequest)
        {
            var tokenRefreshResult = await _authenticationService.RefreshTokenAsync(refreshTokenRequest.Token, refreshTokenRequest.RefreshToken);

            if (!tokenRefreshResult.Success)
                return BadRequest(new AuthenticationResult() { Errors = tokenRefreshResult.Errors });

            return Ok(tokenRefreshResult.CastTo<AuthenticateUserResponse>());
        }
    }
}