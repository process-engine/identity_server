namespace ProcessEngine.IdentityServer.Web.Features.ExternalAuthentication.Provider
{
    using Newtonsoft.Json.Linq;

    public interface IExternalAuthProvider
    {
        string Type { get; }

        UserInfo GetUserInfo(string accessToken);
    }
}
