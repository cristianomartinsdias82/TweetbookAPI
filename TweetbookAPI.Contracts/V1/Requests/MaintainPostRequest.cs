using System;

namespace TweetbookAPI.Contracts.V1.Requests
{
    public class MaintainPostRequest
    {
        public DateTime CreationDate { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime? DisplayUntil { get; set; }
    }
}