using Microsoft.AspNetCore.Authorization;

namespace AspNetCoreIdentity.Infrastructure
{
    internal class StreamingCategoryRequirement: IAuthorizationRequirement
    {
        public string Category { get; private set; }

        public StreamingCategoryRequirement(string category) { Category = category; }
    }
}