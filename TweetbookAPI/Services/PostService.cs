using Microsoft.AspNetCore.Http;
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
        //private readonly IHttpContextAccessor _httpContextAccessor;

        //public PostService(DataContext dataContext, IHttpContextAccessor httpContextAccessor) : base(dataContext)
        //{
        //    _httpContextAccessor = httpContextAccessor;
        //}

        public PostService(DataContext dataContext) : base(dataContext) {}

        public override async Task<bool> UpdateAsync(Post item)
        {
            CheckInstanceAvailability();

            var itemToUpdate = await GetByIdAsync(item.Id);
            if (itemToUpdate == null)
                return await Task.FromResult(false);

            //The infrastructure evaluates for us via PostOwnershipValidationFilter where applied
            //But what if the developer forgets to decorate the endpoint with the attribute aforementioned? In this case, uncomment the line below or adopt a better coding strategy*
            //if (!CurrentUserIsOwner(itemToUpdate.UserId))
            //    return await Task.FromResult(false);
            //*throw new SecurityException("Access denied to the request resource or operation")

            //DataContext.Set<T>().Update(item); //See next line below

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
                //The infrastructure evaluates for us via PostOwnershipValidationFilter where applied
                //But what if the developer forgets to decorate the endpoint with the attribute aforementioned? In this case, uncomment the line below or adopt a better coding strategy*
                //if (!CurrentUserIsOwner(itemToRemove.UserId))
                //    return await Task.FromResult(false);
                //*throw new SecurityException("Access denied to the request resource or operation")

                DataContext.Entry(itemToRemove).State = EntityState.Deleted;
                await DataContext.SaveChangesAsync();
            }

            return await Task.FromResult(true);
        }

        public async Task<IEnumerable<Tag>> GetTagsByPostAsync(int postId)
        {
            //TODO: Implement get tags by post id method
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

        //The infrastructure evaluates for us via PostOwnershipValidationFilter where applied
        //private bool CurrentUserIsOwner(string postUserId) => string.Equals(_httpContextAccessor.HttpContext.GetCurrentUserId(), postUserId, StringComparison.Ordinal);
    }
}
