using System.Collections.Generic;

namespace TweetbookAPI.Services
{
    public static class ResourcePolicies
    {
        private static IDictionary<string, string> Policies;

        static ResourcePolicies()
        {
            Policies = new Dictionary<string, string>()
            {
                [ViewTagPermissionPolicy] = "tags.view",
                [DeletePostPermissionPolicy] = "posts.delete",
                [InternalEmployeePermissionPolicy] = "internalemployee"
            };
        }

        public const string ViewTagPermissionPolicy = "ViewTagPermissionPolicy";
        public const string DeletePostPermissionPolicy = "DeletePostPermissionPolicy";
        public const string InternalEmployeePermissionPolicy = "InternalEmployeePermissionPolicy";

        public static string TryGetPolicyMetaname(string resourcePolicyName)
        {
            string metaname = null;
            Policies.TryGetValue(resourcePolicyName, out metaname);

            return metaname;
        }
    }
}
