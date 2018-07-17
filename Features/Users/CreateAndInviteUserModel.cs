namespace ProcessEngine.IdentityServer.Web.Features.Users
{
    using System.ComponentModel.DataAnnotations;

    public class CreateAndInviteUserModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }
    }
}
