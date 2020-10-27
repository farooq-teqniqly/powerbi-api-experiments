using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using PowerBiBuddy.Client.Models;

namespace PowerBiBuddy
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var token = await GetTokenAsync();
            var client = new PbiClient("power-bi-buddy-console", token, new HttpWebRequestFactory());

            //await CreateWorkspace(client);
            var workspaceId = new Guid("8af1da02-6cb2-4039-9aab-5ef52b62ed3c");
            //await CreateDataset(client, workspaceId);

            var datasetId = new Guid("d3b5bad8-c2c4-467c-b2c9-401bce705a0c");
            //await DeleteDataFromDataset(client);
            await AddDataToDataset(client, workspaceId, datasetId, "Connections");

            // workspace id: 8af1da02-6cb2-4039-9aab-5ef52b62ed3c
            // dataset id: 169988d8-dfe2-4151-84ed-d7c5d5f92191

            Console.Read();
        }

        private static async Task CreateWorkspace(PbiClient client)
        {
            Console.WriteLine("Creating workspace...");

            using (client)
            {
                var response = await client.AddWorkspaceAsync(
                    new AddWorkspaceRequest {Name = "farooq-test-brim"});

                Console.WriteLine("Workspace created.");
                Console.WriteLine(response);
            }
        }

        private static async Task CreateDataset(PbiClient client, Guid workspaceId)
        {
            Console.WriteLine("Creating dataset...");

            using (client)
            {
                var addDatasetRequest = new AddDatasetRequest
                {
                    Name = "ReportFromApis",
                    Tables = new List<Table>
                    {
                        new Table
                        {
                            Name = "Connections",
                            Columns = new List<Column>
                            {
                                new Column {Name = "TenantId", DataType = "string"},
                                new Column {Name = "Type", DataType = "string"},
                                new Column {Name = "ConnectionString", DataType = "string"}
                            }
                        }
                    }
                };

                var response = await client.AddDatasetAsync(
                    workspaceId,
                    addDatasetRequest);
                
                Console.WriteLine("Dataset created.");
                Console.WriteLine(response);
            }

        }

        private static async Task DeleteDataFromDataset(PbiClient client, Guid workspaceId, Guid datasetId)
        {
            Console.WriteLine("Deleting data from dataset...");

            var response = await client.DeleteDataFromDatasetAsync(
                workspaceId,
                datasetId,
                "Connections");

            Console.WriteLine("Data deleted.");
            Console.WriteLine(response);
        }

        private static async Task AddDataToDataset(PbiClient client, Guid workspaceId, Guid datasetId, string tableName)
        {
            Console.WriteLine("Adding data to dataset...");

            var addRowsRequest = new AddDatasetRowsRequest<ConnectionRow>
            {
                Rows = new List<ConnectionRow>
                {
                    new ConnectionRow {TenantId = "tenant1", Type = "sqlserver", ConnectionString = "cs-tenant1"},
                    new ConnectionRow {TenantId = "tenant2", Type = "sqlserver", ConnectionString = "cs-tenant2"},
                    new ConnectionRow {TenantId = "tenant3", Type = "sqlserver", ConnectionString = "cs-tenant3"}
                }
            };

            var response = await client.AddRowsToDatasetAsync(
                workspaceId,
                datasetId,
                tableName,
                addRowsRequest);

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
