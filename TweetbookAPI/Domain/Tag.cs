using System.ComponentModel.DataAnnotations.Schema;

namespace TweetbookAPI.Domain
{
    public class Tag : Entity<int>
    {
        public string Text { get; set; }

        public int? PostId { get; set; }

        [ForeignKey(nameof(PostId))]
        public Post Post { get; set; }
    }
}
