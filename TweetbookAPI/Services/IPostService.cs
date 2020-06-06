using TweetbookAPI.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TweetbookAPI.Services
{
    public interface IPostService : ITweetbookAppService<Post,int>
    {
        Task<IEnumerable<Tag>> GetTagsByPostAsync(int postId);
        Task<Tag> CreatePostTagAsync(Tag tag);
    }
}
