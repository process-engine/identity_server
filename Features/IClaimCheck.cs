namespace ProcessEngine.IdentityServer.Web.Features
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;

    public interface IClaimCheck
    {
        Task<ClaimCheckResult> HasClaim(IHeaderDictionary headers, string claimName, string claimValue = null);
    }
}
