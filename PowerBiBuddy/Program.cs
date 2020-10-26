using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
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
            var client = new PbiClient("power-bi-buddy-console", token, new HttpWebRequestFactory());

            //await CreateWorkspace(client);
            //await CreateDataset(client);
            await DeleteDataFromDataset(client);
            await AddDataToDataset(client);

            // workspace id: 88e4cf51-4b79-4ca1-af8b-a1180e4eaf90
            // dataset id: 04c08958-15ed-4694-a35a-0786386bf3c9
            
            Console.Read();
        }

        private static async Task CreateWorkspace(PbiClient client)
        {
            Console.WriteLine("Creating workspace...");

            using (client)
            {
                var workspaceJson = @"{
                                      ""name"": ""farooq-test-brim""
                                    }";

                var response = await client.AddWorkspaceAsync(workspaceJson);

                Console.WriteLine("Workspace created.");
                Console.WriteLine(response);
            }
        }

        private static async Task CreateDataset(PbiClient client)
        {
            Console.WriteLine("Creating dataset...");

            using (client)
            {
                var datasetJson = @"{
                                      ""name"": ""ReportFromApis"",
                                      ""defaultMode"": ""Push"",
                                      ""tables"": [
                                        {
                                          ""name"": ""Connections"",
                                          ""columns"": [
                                            {
                                              ""name"": ""TenantId"",
                                              ""dataType"": ""string""
                                            },
                                            {
                                              ""name"": ""Type"",
                                              ""dataType"": ""string""
                                            },
                                            {
                                              ""name"": ""ConnectionString"",
                                              ""dataType"": ""string""
                                            }
                                          ]
                                        }
                                      ]
                                    }";

                var response = await client.AddDatasetAsync(Guid.Parse("88e4cf51-4b79-4ca1-af8b-a1180e4eaf90"), datasetJson);
                
                Console.WriteLine("Dataset created.");
                Console.WriteLine(response);
            }

        }

        private static async Task DeleteDataFromDataset(PbiClient client)
        {
            Console.WriteLine("Deleting data from dataset...");

            var response = await client.DeleteDataFromDatasetAsync(
                Guid.Parse("88e4cf51-4b79-4ca1-af8b-a1180e4eaf90"),
                Guid.Parse("04c08958-15ed-4694-a35a-0786386bf3c9"),
                "Connections");

            Console.WriteLine("Data deleted.");
            Console.WriteLine(response);
        }

        private static async Task AddDataToDataset(PbiClient client)
        {
            Console.WriteLine("Adding data to dataset...");

           

            string rowsJson = "{\"rows\":" +
                                  "[{\"TenantId\":\"tenant1\",\"Type\":\"sqlserver\",\"ConnectionString\":\"cs-tenant1\"}," +
                                  "{\"TenantId\":\"tenant2\",\"Type\":\"sqlserver\",\"ConnectionString\":\"cs-tenant2\"}," +
                                  "{\"TenantId\":\"tenant3\",\"Type\":\"sqlserver\",\"ConnectionString\":\"cs-tenant3\"}]}";

            var response = await client.AddDataToDatasetAsync(
                Guid.Parse("88e4cf51-4b79-4ca1-af8b-a1180e4eaf90"),
                Guid.Parse("04c08958-15ed-4694-a35a-0786386bf3c9"),
                "Connections",
                rowsJson);

            Console.WriteLine("Data added.");
            Console.WriteLine(response);
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
            client.DefaultRequestHeaders.Add("Content-Type", "application/json");
            return client;
        }
    }
}
