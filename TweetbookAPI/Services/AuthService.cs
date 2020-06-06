using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.IdentityModel.Tokens;
using TweetbookAPI.ConfigOptions;
using TweetbookAPI.Data;
using TweetbookAPI.DTO;
using TweetbookAPI.Infrastructure;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace TweetbookAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtConfigOptions _jwtConfigOptions;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly DataContext _dataContext;
        private readonly IDateTime _dateTime;

        public AuthService(
            UserManager<IdentityUser> userManager,
            JwtConfigOptions jwtConfigOptions,
            TokenValidationParameters tokenValidationParameters,
            DataContext dataContext,
            IDateTime dateTime)
        {
            _userManager = userManager;
            _jwtConfigOptions = jwtConfigOptions;
            _tokenValidationParameters = tokenValidationParameters;
            _dataContext = dataContext;
            _dateTime = dateTime;
        }

        public async Task<AuthenticationResult> RegisterAsync(string usernameOrEmail, string password)
        {
            var userAlreadyExists = await _userManager.FindByEmailAsync(usernameOrEmail) != null;

            if (userAlreadyExists)
                return new AuthenticationResult()
                {
                    Errors = new[] { "An user with this name/e-mail already exists!" }
                };

            var newUser = new IdentityUser()
            {
                Email = usernameOrEmail,
                UserName = usernameOrEmail
            };

            //User creation attempt logic
            var createdUserResult = await _userManager.CreateAsync(newUser, password);

            if (!createdUserResult.Succeeded)
                return new AuthenticationResult()
                {
                    Errors = createdUserResult.Errors.Select(x => $"{x.Code} - {x.Description}")
                };

            //If user email has a certain domain, he/she is allowed to view post's tags
            //(Hypothetic example of how you can add a claim to some user when registering)
            if (newUser.Email.ToUpperInvariant().EndsWith("@MYCOMPANY.COM.BR"))
                await _userManager.AddClaimAsync(newUser, new Claim(ResourcePolicies.TryGetPolicyMetaname(ResourcePolicies.ViewTagPermissionPolicy), true.ToString()));

            //If user name is fulano.beltrano, then he is king! Otherwise, just another pawn in the chessboard
            //(Hypothetic example of how you can add people to some specific role)
            if (newUser.UserName.ToUpperInvariant().Equals("FULANO.BELTRANO@GMAIL.COM"))
                await _userManager.AddToRoleAsync(newUser, ApplicationRoles.Administrators);
            else
                await _userManager.AddToRoleAsync(newUser, ApplicationRoles.Publishers);

            var tokenCreationResult = await CreateUserTokenAsync(newUser);

            return new AuthenticationResult()
            {
                Success = true,
                Token = tokenCreationResult.NewlyGeneratedToken,
                RefreshToken = tokenCreationResult.RefreshToken.Token
            };
        }

        public async Task<AuthenticationResult> AuthenticateAsync(string usernameOrEmail, string password)
        {
            var user = await _userManager.FindByEmailAsync(usernameOrEmail);

            if (user == null)
                return new AuthenticationResult()
                {
                    Errors = new[] { "Invalid username/e-mail" }
                };

            if (!await _userManager.CheckPasswordAsync(user, password))
                return new AuthenticationResult()
                {
                    Errors = new[] { "Invalid username/e-mail and/or password" }
                };

            var tokenCreationResult = await CreateUserTokenAsync(user);

            return new AuthenticationResult()
            {
                Success = true,
                Token = tokenCreationResult.NewlyGeneratedToken,
                RefreshToken = tokenCreationResult.RefreshToken.Token
            };
        }

        public async Task<AuthenticationResult> RefreshTokenAsync(string token, string refreshTokenId)
        {
            var tokenPrincipal = GetPrincipalFromToken(token);

            if (tokenPrincipal == null)
                return new AuthenticationResult() { Errors = new[] { "Invalid token" } };

            var unixFormatExpirationDateTime = long.Parse(tokenPrincipal.Claims.Single(it => it.Type == JwtRegisteredClaimNames.Exp).Value);
            var utcExpirationDateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(unixFormatExpirationDateTime);

            if (utcExpirationDateTime > _dateTime.Now)
                return new AuthenticationResult() { Errors = new[] { "The informed token is still valid" } };

            var jti = tokenPrincipal.Claims.Single(it => it.Type == JwtRegisteredClaimNames.Jti).Value;

            var storedRefreshToken = _dataContext.RefreshTokens.SingleOrDefault(it => it.Token == refreshTokenId); //The Token property is the key in its table!

            if (storedRefreshToken == null)
                return new AuthenticationResult { Errors = new[] { "The informed refresh token doesn't exist" } };

            if (_dateTime.Now > storedRefreshToken.ExpirationDate)
                return new AuthenticationResult { Errors = new[] { "The informed refresh token has expired" } };

            if (storedRefreshToken.Invalidated)
                return new AuthenticationResult { Errors = new[] { "The informed refresh token has been invalidated" } };

            if (storedRefreshToken.Used)
                return new AuthenticationResult { Errors = new[] { "The informed refresh token has already been used" } };

            if (storedRefreshToken.JwtId != jti)
                return new AuthenticationResult { Errors = new[] { "The informed refresh token does not matched this JWT" } };

            storedRefreshToken.Used = true;
            _dataContext.RefreshTokens.Update(storedRefreshToken);
            await _dataContext.SaveChangesAsync();

            var user = await _userManager.FindByIdAsync(tokenPrincipal.Claims.Single(it => it.Type == "id").Value);

            var tokenCreationResult = await CreateUserTokenAsync(user);
            return new AuthenticationResult()
            {
                Token = tokenCreationResult.NewlyGeneratedToken,
                RefreshToken = tokenCreationResult.RefreshToken.Token,
                Success = true
            };
        }

        private async Task<(string NewlyGeneratedToken, RefreshToken RefreshToken)> CreateUserTokenAsync(IdentityUser user)
        {
            var key = Encoding.ASCII.GetBytes(_jwtConfigOptions.Secret);

            var userClaims = await _userManager.GetClaimsAsync(user) ?? new List<Claim>();
            var essentialClaims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("role", (await _userManager.GetRolesAsync(user)).Join(",")),
                new Claim("id", user.Id),
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(essentialClaims.Concat(userClaims)),
                Expires = _dateTime.Now.Add(_jwtConfigOptions.TokenLifetimeInHours),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            //Visit https://jwt.ms/ to reveal token data
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var newlyGeneratedToken = tokenHandler.WriteToken(token);

            //Creates an entry for Refresh Tokens
            var refreshToken = new RefreshToken()
            {
                //Token = Guid.NewGuid().ToString(), //Check whether AutoGenerated attribute is working! In case it's not working, just uncomment it out and there is gone :)
                JwtId = token.Id,
                UserId = user.Id,
                CreationDate = _dateTime.Now,
                ExpirationDate = _dateTime.Now.AddMonths(_jwtConfigOptions.RefreshTokenLifetimeInMonths)
            };

            await _dataContext.RefreshTokens.AddAsync(refreshToken);
            await _dataContext.SaveChangesAsync(); //Here, the refreshToken's Token property is automatically assigned by the db 'cause the Token property is set as THE KEY!

            return (newlyGeneratedToken, refreshToken);
        }

        private ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var principal = tokenHandler.ValidateToken(token, _tokenValidationParameters, out var validatedToken);

                return JwtSecurityAlgorithmIsValid(validatedToken) ? principal : null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private bool JwtSecurityAlgorithmIsValid(SecurityToken token) => (token is JwtSecurityToken jwtSecurityToken) && jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (_userManager != null)
                        _userManager.Dispose();

                    if (_dataContext != null)
                        _dataContext.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        ~AuthService()
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
