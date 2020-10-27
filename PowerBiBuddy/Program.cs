using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using PowerBiBuddy.Client.Models;

namespace PowerBiBuddy
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("USAGE: onboard [company]");
                return -1;
            }

            var company = args[0];

            var token = await GetTokenAsync();
            var client = new PbiClient("onboard-pbi-console", token, new HttpWebRequestFactory());
            
            using (client)
            {
                // Create workspace
                var workspaceName = $"test-{company}";

                Console.WriteLine($"Creating workspace {workspaceName}...");

                var workspace = await client.AddWorkspaceAsync(new AddWorkspaceRequest { Name = workspaceName });

                Console.WriteLine($"Workspace {workspace.Name} created.");

                var datasetName = "receiving-dataset";
                var tableName = "Connections";

                Console.WriteLine($"Creating dataset {datasetName}...");

                var dataset = await client.AddDatasetAsync(
                    workspace.Id,
                    new AddDatasetRequest
                    {
                        Name = datasetName,
                        Tables = new List<Table>
                        {
                        new Table
                        {
                            Name = tableName,
                            Columns = new List<Column>
                            {
                                new Column {Name = "TenantId", DataType = "string"},
                                new Column {Name = "Type", DataType = "string"},
                                new Column {Name = "ConnectionString", DataType = "string"}
                            }
                        }
                        }
                    });

                Console.WriteLine($"Dataset {dataset.Name} created.");

                Console.WriteLine($"Populating dataset {dataset.Name}...");

                var addRowsRequest = new AddDatasetRowsRequest<ConnectionRow>
                {
                    Rows = new List<ConnectionRow>
                {
                    new ConnectionRow {TenantId = "tenant1", Type = "sqlserver", ConnectionString = "cs-tenant1"},
                    new ConnectionRow {TenantId = "tenant2", Type = "sqlserver", ConnectionString = "cs-tenant2"},
                    new ConnectionRow {TenantId = "tenant3", Type = "sqlserver", ConnectionString = "cs-tenant3"}
                }
                };

                await client.AddRowsToDatasetAsync(
                    workspace.Id,
                    dataset.Id,
                    tableName,
                    addRowsRequest);

                Console.WriteLine($"Finished populating dataset {dataset.Name}.");

                var masterWorkspaceName = "test-erep-master";
                var masterWorkspace = (await client.GetWorkspacesAsync()).Values.Single(w => w.Name == masterWorkspaceName);
                var masterReport = (await client.GetReportsAsync(masterWorkspace.Id)).Values.Single();

                Console.WriteLine($"Cloning report {masterReport.Name}...");

                var clonedReport = await client.CloneReportAsync(
                    masterWorkspace.Id, 
                    masterReport.Id, 
                    workspace.Id, 
                    dataset.Id, 
                    masterReport.Name);

                Console.WriteLine($"Finished cloning report {clonedReport.Name}");

                Console.Read();

                return 0; 
            }
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
    }
}
