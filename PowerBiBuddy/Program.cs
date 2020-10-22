using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace PowerBiBuddy
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var token = await GetTokenAsync();
            var client = new PbiClient("power-bi-buddy-console", token);

            using (client)
            {
                var workspaces = await client.GetWorkspacesAsync();
                Console.WriteLine(workspaces);
            }

            Console.Read();
        }

        private static async Task<string> GetTokenAsync()
        {
            var authResult = await LoginInteractive();
            var token = authResult.CreateAuthorizationHeader();
            return token;
        }

        private static async Task<AuthenticationResult> LoginInteractive()
        {
            var authContext = new AuthenticationContext("https://login.windows.net/common/oauth2/authorize");

            var authResult = await authContext.AcquireTokenAsync(
                "https://analysis.windows.net/powerbi/api",
                "3120e4df-4b0d-4be9-b117-dc835aa84646",
                new Uri("https://dev.powerbi.com/Apps/SignInRedirect"),
                new PlatformParameters(PromptBehavior.Always));

            return authResult;
        }

        private static HttpClient CreateHttpClient(string authToken)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", authToken);
            return client;
        }
    }
}
