using System;

namespace TweetbookAPI.ConfigOptions
{
    public class JwtConfigOptions
    {
        public string Secret { get; set; }
        public int RefreshTokenLifetimeInMonths { get; set; }
        public TimeSpan TokenLifetimeInHours { get; set; }
    }
}
