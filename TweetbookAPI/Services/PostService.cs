using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TweetbookAPI.Data;
using TweetbookAPI.Domain;
using TweetbookAPI.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TweetbookAPI.Services
{
    public class PostService : TweetbookAppService<Post>, IPostService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PostService(DataContext dataContext, IHttpContextAccessor httpContextAccessor) : base(dataContext)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public override async Task<bool> UpdateAsync(Post item)
        {
            CheckInstanceAvailability();

            var itemToUpdate = await GetByIdAsync(item.Id);
            if (itemToUpdate == null)
                return await Task.FromResult(false);

            if (!CurrentUserIsOwner(itemToUpdate.UserId))
                return await Task.FromResult(false);

            //DataContext.Set<T>().Update(item);

            //https://stackoverflow.com/questions/7106211/entity-framework-why-explicitly-set-entity-state-to-modified
            item.UserId = itemToUpdate.UserId;
            DataContext.Entry(itemToUpdate).CurrentValues.SetValues(item);

            var updated = await DataContext.SaveChangesAsync() > 0;

            return await Task.FromResult(updated);
        }

        public override async Task<bool> RemoveAsync(int id)
        {
            CheckInstanceAvailability();

            var itemToRemove = await GetByIdAsync(id);
            if (itemToRemove != null)
            {
                if (!CurrentUserIsOwner(itemToRemove.UserId))
                    return await Task.FromResult(false);

                DataContext.Entry(itemToRemove).State = EntityState.Deleted;
                await DataContext.SaveChangesAsync();
            }

            return await Task.FromResult(true);
        }

        public async Task<IEnumerable<Tag>> GetTagsByPostAsync(int postId)
        {
            CheckInstanceAvailability();

            return await Task.FromResult(Enumerable.Empty<Tag>());
        }

        public async Task<Tag> CreatePostTagAsync(Tag tag)
        {
            CheckInstanceAvailability();

            var relatedPost = await GetByIdAsync(tag.PostId.Value);
            if (relatedPost != null)
            {
                await DataContext.Tags.AddAsync(tag);
                var created = await DataContext.SaveChangesAsync() > 0;

                return await Task.FromResult(created ? tag : null);
            }

            return await Task.FromResult((Tag)null);
        }

        private bool CurrentUserIsOwner(string postUserId) => string.Equals(_httpContextAccessor.HttpContext.GetCurrentUserId(), postUserId, StringComparison.Ordinal);
    }
}
