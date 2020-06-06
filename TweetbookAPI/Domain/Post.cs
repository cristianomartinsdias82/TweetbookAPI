using TweetbookAPI.Controllers.V1;
using System;
using System.Collections.Generic;

namespace TweetbookAPI.Domain
{
    public class Post : Entity<int>
    {
        public DateTime CreationDate { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime? DisplayUntil { get; set; }
        public string UserId { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
    }
}