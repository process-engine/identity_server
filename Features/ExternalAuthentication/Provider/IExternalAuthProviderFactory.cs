namespace ProcessEngine.IdentityServer.Web.Features.ExternalAuthentication.Provider
{
    public interface IExternalAuthProviderFactory
    {
        IExternalAuthProvider Create(string providerType);
    }
}
