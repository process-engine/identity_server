namespace ProcessEngine.IdentityServer.Web.Features.ExternalAuthentication.Provider
{
    using System.Collections.Generic;
    using System.Net.Http;

    public class ExternalAuthProviderFactory : IExternalAuthProviderFactory
    {
        private readonly IDictionary<string, IExternalAuthProvider> externalAuthProviderByType = new Dictionary<string, IExternalAuthProvider>();

        public ExternalAuthProviderFactory()
        {
            var httpClient = new HttpClient();
            var facebook = new FacebookAuthProvider(httpClient);

            this.externalAuthProviderByType = new Dictionary<string, IExternalAuthProvider>()
            {
                {facebook.Type, facebook}
            };
        }
        public IExternalAuthProvider Create(string providerType)
        {
            return this.externalAuthProviderByType[providerType];
        }
    }
}
