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
        public ClaimsController(IClaimCheck claimCheck)
        {
            this.ClaimCheck = claimCheck;
        }

        private IClaimCheck ClaimCheck { get; set; }

        [HttpGet("ensure/{claimName}")]
        public async Task<IActionResult> EnsureHasClaimAsync(string claimName, string claimValue)
        {
            ClaimCheckResult result = await this.ClaimCheck.HasClaim(this.HttpContext.Request.Headers, claimName, claimValue);

            if (result.Success)
            {
                return this.NoContent();
            }
            else
            {
                var responseFeature = this.Response.HttpContext.Features.Get<IHttpResponseFeature>();
                responseFeature.ReasonPhrase = result.ErrorMessage;

                return this.StatusCode((int)result.StatuCode);
            }
        }
    }
}
