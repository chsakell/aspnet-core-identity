using AspNetCoreIdentity.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace AspNetCoreIdentity.Infrastructure
{
    public class AppUserStore : UserStoreBase<IdentityUser, string, IdentityUserClaim<string>,
        IdentityUserLogin<string>, IdentityUserToken<string>>
    {
        public AppUserStore(IdentityErrorDescriber describer) : base(describer)
        {
        }

        public override IQueryable<IdentityUser> Users => throw new NotImplementedException();

        public override Task AddClaimsAsync(IdentityUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public override Task AddLoginAsync(IdentityUser user, UserLoginInfo login, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public override Task<IdentityResult> CreateAsync(IdentityUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public override Task<IdentityResult> DeleteAsync(IdentityUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public override Task<IdentityUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public override Task<IdentityUser> FindByIdAsync(string userId, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public override Task<IdentityUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public override Task<IList<Claim>> GetClaimsAsync(IdentityUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public override Task<IList<UserLoginInfo>> GetLoginsAsync(IdentityUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public override Task<IList<IdentityUser>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public override Task RemoveClaimsAsync(IdentityUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public override Task RemoveLoginAsync(IdentityUser user, string loginProvider, string providerKey, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public override Task ReplaceClaimAsync(IdentityUser user, Claim claim, Claim newClaim, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public override Task<IdentityResult> UpdateAsync(IdentityUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        protected override Task AddUserTokenAsync(IdentityUserToken<string> token)
        {
            throw new NotImplementedException();
        }

        protected override Task<IdentityUserToken<string>> FindTokenAsync(IdentityUser user, string loginProvider, string name, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        protected override Task<IdentityUser> FindUserAsync(string userId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        protected override Task<IdentityUserLogin<string>> FindUserLoginAsync(string userId, string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        protected override Task<IdentityUserLogin<string>> FindUserLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        protected override Task RemoveUserTokenAsync(IdentityUserToken<string> token)
        {
            throw new NotImplementedException();
        }
    }
}
