using System;

namespace TweetbookAPI.Infrastructure
{
    public interface IDateTime
    {
        DateTime Now { get; }
    }
}
