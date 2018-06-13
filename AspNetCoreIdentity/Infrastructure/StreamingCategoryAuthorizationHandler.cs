using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreIdentity.Infrastructure {
    internal class StreamingCategoryAuthorizationHandler : AuthorizationHandler<StreamingCategoryRequirement> {
        private readonly UserManager<IdentityUser> _userManager;

        public StreamingCategoryAuthorizationHandler (UserManager<IdentityUser> userManager) {
            _userManager = userManager;
        }

        protected override Task HandleRequirementAsync (AuthorizationHandlerContext context, StreamingCategoryRequirement requirement) {

            var loggedInUserTask = _userManager.GetUserAsync (context.User);

            loggedInUserTask.Wait ();
              
            var userClaimsTask = _userManager.GetClaimsAsync (loggedInUserTask.Result);

            userClaimsTask.Wait ();

            var userClaims = userClaimsTask.Result;

            if (userClaims.Any (c => c.Type == requirement.Category)) {
                context.Succeed (requirement);
            }

            return Task.CompletedTask;
        }
    }
}