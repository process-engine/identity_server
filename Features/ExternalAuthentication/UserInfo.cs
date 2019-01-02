namespace ProcessEngine.IdentityServer.Web.Features.ExternalAuthentication
{
    public class UserInfo
    {
        public UserInfo(string id, string email)
        {
            this.Id = id;
            this.Email = email;
        }

        public string Id { get; }

        public string Email { get; }
    }
}
