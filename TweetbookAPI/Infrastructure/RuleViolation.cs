using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TweetbookAPI.Infrastructure
{
    public class RuleViolation
    {
        public string Field { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class RuleViolationCollection : List<RuleViolation> { }
}
