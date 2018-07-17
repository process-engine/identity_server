namespace ProcessEngine.IdentityServer.Web.Features
{
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using IdentityServer4.Validation;
    using Microsoft.AspNetCore.Http;

    public class ClaimCheck : IClaimCheck
    {
        public ClaimCheck(ITokenValidator tokenValidator)
        {
            this.TokenValidator = tokenValidator;
        }

        private ITokenValidator TokenValidator { get; set; }

        public async Task<ClaimCheckResult> HasClaim(IHeaderDictionary headers, string claimName, string claimValue)
        {
            var hasNoAuthorizationHeader = !headers.ContainsKey("Authorization");
            if (hasNoAuthorizationHeader)
            {
                return ClaimCheckResult.Failed("No Authorization Header found", HttpStatusCode.Unauthorized);
            }

            var (tokenType, token) = this.GetAuthorizationHeaderTypeAndToken(headers);

            var isNoBearerToken = !tokenType.Equals("Bearer");
            if (isNoBearerToken)
            {
                return ClaimCheckResult.Failed("No valid access token supplied", HttpStatusCode.Unauthorized);
            }

            var validationResult = await this.TokenValidator.ValidateAccessTokenAsync(token);

            var isInvalidToken = validationResult.IsError;
            if (isInvalidToken)
            {
                return ClaimCheckResult.Failed("Invalid access token.", HttpStatusCode.Unauthorized);
            }

            var claims = validationResult.Claims;

            var matchingClaim = claims.FirstOrDefault(currentClaim => currentClaim.Type.Equals(claimName));
            if (matchingClaim == null)
            {
                return ClaimCheckResult.Failed($"Identity has no claim '{claimName}'.", HttpStatusCode.Forbidden);
            }

            if (!string.IsNullOrEmpty(claimValue) && matchingClaim.Value != claimValue)
            {
                return ClaimCheckResult.Failed($"Identity claim '{claimName}' does not have the required value.", HttpStatusCode.Forbidden);
            }

            return ClaimCheckResult.Succeeded();
        }

        private (string, string) GetAuthorizationHeaderTypeAndToken(IHeaderDictionary headers)
        {
            var authorizationHeader = headers["Authorization"];
            var authInfo = authorizationHeader.ToString().Split(' ');

            return (authInfo[0], authInfo[1]);
        }
    }
}
