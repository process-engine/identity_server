namespace ProcessEngine.IdentityServer.Web.Features
{
    using System.Net;

    public class ClaimCheckResult
    {
        private ClaimCheckResult(bool success, string errorMessage, HttpStatusCode statusCode)
        {
            this.Success = success;
            this.ErrorMessage = errorMessage;
            this.StatuCode = statusCode;
        }

        public static ClaimCheckResult Succeeded()
        {
            return new ClaimCheckResult(true, string.Empty, HttpStatusCode.OK);
        }

        public static ClaimCheckResult Failed(string errorMessage, HttpStatusCode statusCode)
        {
            return new ClaimCheckResult(false, errorMessage, statusCode);
        }

        public bool Success { get; private set; }

        public string ErrorMessage { get; private set; }

        public HttpStatusCode StatuCode { get; private set; }
    }

}
