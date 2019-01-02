namespace ProcessEngine.IdentityServer.Web.Features.ExternalAuthentication
{
    using IdentityServer4.Models;
    using IdentityServer4.Validation;
    using Microsoft.AspNetCore.Identity;
    using ProcessEngine.IdentityServer.Web.Features.ExternalAuthentication.Provider;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ExternalAuthenticationGrant<TUser> : IExtensionGrantValidator
        where TUser : IdentityUser<string>, new()
    {
        private readonly UserManager<TUser> userManager;
        private readonly IExternalAuthProviderFactory externalAuthProviderFactory;
        public ExternalAuthenticationGrant(
            UserManager<TUser> userManager,
            IExternalAuthProviderFactory externalAuthProviderFactory
            )
        {
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            this.externalAuthProviderFactory = externalAuthProviderFactory ?? throw new ArgumentNullException(nameof(externalAuthProviderFactory));
        }

        public string GrantType => "external";

        public async Task ValidateAsync(ExtensionGrantValidationContext context)
        {
            var provider = context.Request.Raw.Get("provider");
            if (string.IsNullOrWhiteSpace(provider))
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidRequest, "invalid provider");
                return;
            }


            var token = context.Request.Raw.Get("external_token");
            if (string.IsNullOrWhiteSpace(token))
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidRequest, "invalid external token");
                return;
            }


            var externalAuthProvider = this.externalAuthProviderFactory.Create(provider);

            if (externalAuthProvider == null)
            {

                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidRequest, "invalid provider type");
                return;
            }

            var userInfo = externalAuthProvider.GetUserInfo(token);

            if (userInfo == null)
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidRequest, "couldn't retrieve user info from specified provider, please make sure that access token is not expired.");
                return;
            }

            var externalId = userInfo.Id;
            if (!string.IsNullOrWhiteSpace(externalId))
            {

                var user = await this.userManager.FindByLoginAsync(provider, externalId);
                if (null != user)
                {
                    user = await this.userManager.FindByIdAsync(user.Id);
                    var userClaims = await this.userManager.GetClaimsAsync(user);
                    context.Result = new GrantValidationResult(user.Id, provider, userClaims, provider, null);
                    return;
                }
            }
        }
    }
}
