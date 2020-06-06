using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using TweetbookAPI.Data;
using TweetbookAPI.Domain;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;

namespace TweetbookAPI.Services
{
    public abstract class TweetbookAppService<T> : ITweetbookAppService<T, int> where T : Entity<int>
    {
        protected DataContext DataContext { get; private set; }

        public TweetbookAppService(DataContext dataContext)
        {
            DataContext = dataContext;
        }

        public async virtual Task<IEnumerable<T>> GetAllAsync()
        {
            CheckInstanceAvailability();

            return await Task.FromResult(DataContext.Set<T>().ToImmutableList());
        }

        public async virtual Task<T> GetByIdAsync(int id)
        {
            CheckInstanceAvailability();

            return await DataContext.Set<T>().FirstOrDefaultAsync(it => it.Id == id);
        }

        public async virtual Task<bool> CreateAsync(T item)
        {
            CheckInstanceAvailability();

            await DataContext.Set<T>().AddAsync(item);
            var created = await DataContext.SaveChangesAsync() > 0;

            return await Task.FromResult(created);
        }

        public async virtual Task<bool> RemoveAsync(int id)
        {
            CheckInstanceAvailability();

            var itemToRemove = await GetByIdAsync(id);
            if (itemToRemove != null)
            {
                DataContext.Entry(itemToRemove).State = EntityState.Deleted;
                await DataContext.SaveChangesAsync();
            }

            return await Task.FromResult(true);
        }

        public async virtual Task<bool> UpdateAsync(T item)
        {
            CheckInstanceAvailability();

            //DataContext.Set<T>().Update(item);

            var itemToUpdate = await DataContext.Set<T>().SingleOrDefaultAsync(it => it.Id == item.Id);
            if (itemToUpdate == null)
                return await Task.FromResult(false);

            //https://stackoverflow.com/questions/7106211/entity-framework-why-explicitly-set-entity-state-to-modified
            DataContext.Entry(itemToUpdate).CurrentValues.SetValues(item);

            var updated = await DataContext.SaveChangesAsync() > 0;

            return await Task.FromResult(updated);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (DataContext != null)
                        DataContext.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        ~TweetbookAppService()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

        protected void CheckInstanceAvailability()
        {
            if (disposedValue)
                throw new ObjectDisposedException("This service instance was disposed and is no longer available!");
        }
    }
}
