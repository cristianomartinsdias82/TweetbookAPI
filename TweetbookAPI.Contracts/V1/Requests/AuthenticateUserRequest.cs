namespace TweetbookAPI.Contracts.V1.Requests
{
    public class AuthenticateUserRequest
    {
        public string UsernameOrEmail { get; set; }
        public string Password { get; set; }
    }
}
