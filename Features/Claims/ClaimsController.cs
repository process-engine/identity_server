namespace ProcessEngine.IdentityServer.Web.Features.Claims
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;

    using IdentityServer4.Validation;
    using Microsoft.AspNetCore.Cors;
    using Microsoft.AspNetCore.Http.Features;
    using Microsoft.AspNetCore.Mvc;

    [Route("/claims")]
    public class ClaimsController : Controller
    {
        public ClaimsController(ITokenValidator tokenValidator)
        {
            this.TokenValidator = tokenValidator;
        }

        private ITokenValidator TokenValidator { get; set; }

        [HttpGet("ensure/{claim}")]
        public async Task<IActionResult> EnsureHasClaimAsync(string claim)
        {
            var headers = this.HttpContext.Request.Headers;

            var hasNoAuthorizationHeader = !headers.ContainsKey("Authorization");
            if (hasNoAuthorizationHeader)
            {
                return this.Unauthorized();
            }

            var authorizationHeader = headers["Authorization"];
            var authInfo = authorizationHeader.ToString().Split(' ');

            var isNoBearerToken = authInfo.Length != 2 && !authInfo[0].Equals("Bearer");
            if (isNoBearerToken)
            {
                var responseFeature = this.Response.HttpContext.Features
                    .Get<IHttpResponseFeature>();
                responseFeature.ReasonPhrase = "No valid access token supplied.";

                return this.Unauthorized();
            }

            var accessToken = authInfo[1];
            var validationResult = await this.TokenValidator.ValidateAccessTokenAsync(accessToken);

            var isInvalidToken = validationResult.IsError;
            if (isInvalidToken)
            {
                var responseFeature = this.Response.HttpContext.Features
                    .Get<IHttpResponseFeature>();
                responseFeature.ReasonPhrase = "Invalid access token.";

                return this.Unauthorized();
            }

            var claims = validationResult.Claims;

            var isClaimMissing = !claims.Any(currentClaim => currentClaim.Type.Equals(claim));
            if (isClaimMissing)
            {
                var responseFeature = this.Response.HttpContext.Features
                    .Get<IHttpResponseFeature>();
                responseFeature.ReasonPhrase = $"Identity has no claim '{claim}'.";

                return this.StatusCode((int)System.Net.HttpStatusCode.Forbidden);
            }

            return this.NoContent();
        }
    }
}
