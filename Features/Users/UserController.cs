namespace ProcessEngine.IdentityServer.Web.Features.Users
{
    using System;
    using System.Net;
    using System.Net.Mail;
    using System.Threading.Tasks;
    using IdentityExpress.Identity;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http.Features;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;

    [Route("/users")]
    public class UsersController : Controller
    {
        private readonly UserManager<IdentityExpressUser> userManager;

        private readonly IdentityExpressDbContext identityExpressDbContext;

        private readonly IClaimCheck claimCheck;

        public UsersController(UserManager<IdentityExpressUser> userManager, IdentityExpressDbContext identityExpressDbContext, IClaimCheck claimCheck)
        {
            this.userManager = userManager;
            this.identityExpressDbContext = identityExpressDbContext;
            this.claimCheck = claimCheck;
        }

        /// <summary>
        /// Creates an inactive User and invites by email
        /// </summary>
        [HttpPost("createandinviteuser")]
        public async Task<IActionResult> CreateAndInviteUser([FromBody] CreateAndInviteUserModel model)
        {
            if (ModelState.IsValid)
            {
                ClaimCheckResult claimCheckResult = await this.claimCheck.HasClaim(HttpContext.Request.Headers, "can_create_local_admin");

                if (!claimCheckResult.Success)
                {
                    var responseFeature = this.Response.HttpContext.Features.Get<IHttpResponseFeature>();
                    responseFeature.ReasonPhrase = claimCheckResult.ErrorMessage;

                    return this.StatusCode((int)claimCheckResult.StatuCode);
                }

                var user = new IdentityExpressUser() { Email = model.Email, UserName = model.Name };
                var result = await this.userManager.CreateAsync(user);
                await this.identityExpressDbContext.SaveChangesAsync();

                if (result.Succeeded)
                {
                    var userId = await this.userManager.GetUserIdAsync(user);
                    await SendInvitationMail(user, userId);

                    return Json(new { userId = userId });
                }
            }

            return BadRequest();
        }

        [HttpGet("completeregistration")]
        public IActionResult CompleteRegistration(string token, string userId)
        {
            var model = new CompleteRegistrationViewModel() { Token = token, UserId = userId };

            return View(model);
        }

        [HttpPost("completeregistration")]
        public async Task<IActionResult> CompleteRegistration(CompleteRegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                byte[] decodedBytes = Convert.FromBase64String(model.Token);
                var token = System.Text.Encoding.Unicode.GetString(decodedBytes);

                var user = await this.userManager.FindByIdAsync(model.UserId);
                var result = await this.userManager.ResetPasswordAsync(user, token, model.Password);
                await this.identityExpressDbContext.SaveChangesAsync();

                if (result.Succeeded)
                {
                    return View("RegistrationCompleted");
                }

                AddErrors(result);
            }

            return View(model);
        }

        private async Task SendInvitationMail(IdentityExpressUser user, string userId)
        {
            var token = await this.userManager.GeneratePasswordResetTokenAsync(user);
            await this.identityExpressDbContext.SaveChangesAsync();

            byte[] encodedBytes = System.Text.Encoding.Unicode.GetBytes(token);
            token = Convert.ToBase64String(encodedBytes);

            var configuration = new ConfigurationBuilder().AddEnvironmentVariables().Build();

            SmtpClient client = new SmtpClient(configuration["SMTP_SERVER"]);
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(configuration["SMTP_USER"], configuration["SMTP_PASSWORD"]);

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("test@5minds.de");
            mailMessage.To.Add(user.Email);
            mailMessage.Body = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/users/completeRegistration?token={token}&userId={userId}";
            mailMessage.Subject = "Registrierung";
            client.Send(mailMessage);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
    }
}
