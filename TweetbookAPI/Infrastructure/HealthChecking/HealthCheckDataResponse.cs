using System;
using System.Collections.Generic;

namespace TweetbookAPI.Infrastructure.HealthChecking
{
    public class HealthCheckDataResponse
    {
        public string Status { get; set; }
        public IEnumerable<HealthCheckData> Data {get; set;}
        public TimeSpan Duration { get; set; }
    }
}
