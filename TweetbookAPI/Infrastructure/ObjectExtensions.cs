using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;

namespace TweetbookAPI.Infrastructure
{
    public static class ObjectExtensions
    {
        public static TOutput CastTo<TOutput>(this object instance, IMapper mapper = null)
        {
            if (mapper == null)
                mapper = GetMapper();

            return mapper.Map<TOutput>(instance);
        }

        public static IEnumerable<TOutput> BatchCastTo<TOutput>(this IEnumerable<object> instance, IMapper mapper = null)
        {
            if (mapper == null)
                mapper = GetMapper();

            return instance.Select(it => mapper.Map<TOutput>(it)).ToList();
        }

        //https://stackoverflow.com/questions/36027206/how-to-use-di-inside-a-static-method-in-asp-net-core-rc1
        private static IMapper GetMapper() => new HttpContextAccessor().HttpContext.RequestServices.GetRequiredService<IMapper>();
    }
}
