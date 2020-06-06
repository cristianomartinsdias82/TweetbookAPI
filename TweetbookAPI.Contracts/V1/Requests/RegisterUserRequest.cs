namespace TweetbookAPI.Contracts.V1.Requests
{
    public class RegisterUserRequest
    {
        public string UsernameOrEmail { get; set; }
        public string Password { get; set; }
    }
}
