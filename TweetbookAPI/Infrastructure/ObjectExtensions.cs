using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace TweetbookAPI.Infrastructure
{
    public static class ObjectExtensions
    {
        public static TOutput CastTo<TOutput>(this object instance)
        {
            //https://stackoverflow.com/questions/36027206/how-to-use-di-inside-a-static-method-in-asp-net-core-rc1
            var mapper = new HttpContextAccessor().HttpContext.RequestServices.GetRequiredService<IMapper>();

            return mapper.Map<TOutput>(instance);
        }
    }
}
