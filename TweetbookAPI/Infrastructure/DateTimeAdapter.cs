using System;

namespace TweetbookAPI.Infrastructure
{
    public class DateTimeAdapter : IDateTime
    {
        public DateTime Now => DateTime.UtcNow;
        //public DateTime Now => DateTime.Now;
    }
}
