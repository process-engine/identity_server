namespace ProcessEngine.IdentityServer.Web.Features.Users
{
    using System.ComponentModel.DataAnnotations;

    public class CompleteRegistrationViewModel
    {
        [Required]
        public string Token { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required(ErrorMessage = "Passwort ist ein Pflichtfeld")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Passwort")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Passwort bestätigen")]
        [Compare("Password", ErrorMessage = "Die Passwörter stimmen nicht überein.")]
        public string ConfirmPassword { get; set; }
    }
}
