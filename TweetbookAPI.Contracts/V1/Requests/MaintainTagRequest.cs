namespace TweetbookAPI.Contracts.V1.Requests
{
    public class MaintainTagRequest
    {
        public int Id { get; set; }
        public string Text { get; set; }

        public int PostId { get; set; }
    }
}
