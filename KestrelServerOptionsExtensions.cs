namespace Microsoft.AspNetCore.Hosting
{
    using System.IO;
    using System.Net;
    using System.Security.Cryptography.X509Certificates;
    using Microsoft.AspNetCore.Server.Kestrel.Core;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class KestrelServerOptionsExtensions
    {
        public static void ConfigureHTTPS(this KestrelServerOptions options)
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var environment = options.ApplicationServices.GetRequiredService<IHostingEnvironment>();

            var builder = new ConfigurationBuilder()
                .SetBasePath(currentDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            var configuration = builder.Build();

            if (!string.IsNullOrEmpty(configuration["CERT_PATH"]) && !string.IsNullOrEmpty(configuration["CERT_PASSWORD"]))
            {
                options.Listen(IPAddress.Any, 5000,
                            listenOptions =>
                            {
                                var certificate = new X509Certificate2(configuration["CERT_PATH"], configuration["CERT_PASSWORD"]);
                                listenOptions.UseHttps(certificate);
                            }
                    );
            }
            else
            {
                options.Listen(IPAddress.Any, 5000);
            }
        }
    }
}
