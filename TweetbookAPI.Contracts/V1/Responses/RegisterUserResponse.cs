namespace TweetbookAPI.Contracts.V1.Responses
{
    public class RegisterUserResponse
    {
        //Just for simplicity purposes! No e-mail validation process will be involved here
        public string Token { get; set; }
    }
}
