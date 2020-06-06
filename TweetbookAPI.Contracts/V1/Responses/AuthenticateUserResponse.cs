using System.Collections.Generic;

namespace TweetbookAPI.Contracts.V1.Responses
{
    public class AuthenticateUserResponse
    {
        public bool Success { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}
