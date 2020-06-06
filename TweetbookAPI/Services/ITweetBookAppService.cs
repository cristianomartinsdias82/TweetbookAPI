using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TweetbookAPI.Services
{
    public interface ITweetbookAppService<T, TKey> where T : class
    {
        Task<T> GetByIdAsync(TKey id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<bool> CreateAsync(T item);
        Task<bool> UpdateAsync(T item);
        Task<bool> RemoveAsync(TKey id);
    }
}
