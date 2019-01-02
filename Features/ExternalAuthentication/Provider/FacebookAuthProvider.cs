namespace ProcessEngine.IdentityServer.Web.Features.ExternalAuthentication.Provider
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Newtonsoft.Json.Linq;
    using System.Net.Http;
    using Microsoft.AspNetCore.Identity;

    public class FacebookAuthProvider : IExternalAuthProvider
    {
        private const string UserInfoEndPoint = "https://graph.facebook.com/v2.8/me";
        private readonly HttpClient _httpClient;
        public FacebookAuthProvider(
            HttpClient httpClient
            )
        {
            _httpClient = httpClient;
        }

        public string Type => "facebook";

        public UserInfo GetUserInfo(string accessToken)
        {
            var result = _httpClient.GetAsync(UserInfoEndPoint + this.GetQuery(accessToken)).Result;
            if (result.IsSuccessStatusCode)
            {
                var infoObject = JObject.Parse(result.Content.ReadAsStringAsync().Result);
                return new UserInfo(infoObject.Value<string>("id"), infoObject.Value<string>("email"));
            }
            return null;
        }

        private string GetQuery(string accessToken)
        {
            return $"?fields=id,email&access_token={accessToken}";
        }
    }
}
