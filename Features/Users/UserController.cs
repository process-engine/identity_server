namespace ProcessEngine.IdentityServer.Web.Features.Users
{
    using System.Net;
    using System.Net.Mail;
    using System.Threading.Tasks;
    using IdentityExpress.Identity;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    [Route("/users")]
    public class UsersController : Controller
    {
        private readonly UserManager<IdentityExpressUser> _userManager;

        public UsersController(UserManager<IdentityExpressUser> userManager)
        {
            this._userManager = userManager;
        }

        /// <summary>
        /// Creates an inactive User and invites by email
        /// </summary>
        [HttpPost("createandinviteuser")]
        public async Task<IActionResult> CreateAndInviteUser([FromBody] CreateAndInviteUserModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityExpressUser() { Email = model.Email, UserName = model.Name };
                var result = await this._userManager.CreateAsync(user);

                if (result.Succeeded)
                {
                    var userId = await this._userManager.GetUserIdAsync(user);
                    await SendInvitationMail(user, userId);

                    return Json(new { userId = userId });
                }
            }

            return BadRequest();
        }

        private async Task SendInvitationMail(IdentityExpressUser user, string userId)
        {
            var token = await this._userManager.GeneratePasswordResetTokenAsync(user);

            SmtpClient client = new SmtpClient("smtp.sendgrid.net");
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("azure_5b2ca743ae2193df8f23f0da2c015e74@azure.com", "test123!");

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("test@5minds.de");
            mailMessage.To.Add(user.Email);
            mailMessage.Body = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/users/completeRegistration?token={token}&userId={userId}";
            mailMessage.Subject = "Registrierung";
            client.Send(mailMessage);
        }
    }
}
