using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace AspNetCoreIdentity.Infrastructure {
    public class StreamingCategoryPolicyProvider : IAuthorizationPolicyProvider {
        const string POLICY_PREFIX = "StreamingCategory_";

        public DefaultAuthorizationPolicyProvider FallbackPolicyProvider { get; }

        public StreamingCategoryPolicyProvider (IOptions<AuthorizationOptions> options) {
            // ASP.NET Core only uses one authorization policy provider, so if the custom implementation
            // doesn't handle all policies (including default policies, etc.) it should fall back to an
            // alternate provider.
            //
            // In this sample, a default authorization policy provider (constructed with options from the 
            // dependency injection container) is used if this custom provider isn't able to handle a given
            // policy name.
            //
            // If a custom policy provider is able to handle all expected policy names then, of course, this
            // fallback pattern is unnecessary.

            options.Value.AddPolicy ("TrialOnly", policy => {
                policy.RequireClaim ("Trial");
            });

            // Role based authorization
            options.Value.AddPolicy ("AdminOnly", policy => {
                policy.RequireRole ("Admin");
            });

            FallbackPolicyProvider = new DefaultAuthorizationPolicyProvider (options);
        }

        public Task<AuthorizationPolicy> GetPolicyAsync (string policyName) {
            if (policyName.StartsWith (POLICY_PREFIX, StringComparison.OrdinalIgnoreCase)) {
                var category = (StreamingCategory) Enum.Parse (typeof (StreamingCategory),
                    policyName.Substring (POLICY_PREFIX.Length));

                var policy = new AuthorizationPolicyBuilder ();
                policy.AddRequirements(new StreamingCategoryRequirement(category.ToString ()));
                return Task.FromResult (policy.Build ());
            } else {
                // If the policy name doesn't match the format expected by this policy provider,
                // try the fallback provider. If no fallback provider is used, this would return 
                // Task.FromResult<AuthorizationPolicy>(null) instead.
                return FallbackPolicyProvider.GetPolicyAsync (policyName);
            }
        }

        public Task<AuthorizationPolicy> GetDefaultPolicyAsync () => FallbackPolicyProvider.GetDefaultPolicyAsync ();

    }
}