using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace PowerBiBuddy
{
    public class PbiClient : IDisposable
    {
        private bool disposed = false;
        private readonly HttpClient httpClient;

        public PbiClient(string userAgent, string authToken)
        {
            this.httpClient = new HttpClient {BaseAddress = new Uri("https://api.powerbi.com/v1.0/myorg/")};
            this.httpClient.DefaultRequestHeaders.Add("User-Agent", userAgent);
            this.httpClient.DefaultRequestHeaders.Add("Authorization", authToken);
        }

        public async Task<string> GetWorkspacesAsync()
        {
            var response = await this.httpClient.GetAsync("groups");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();

            var jo = JObject.Parse(json);

            return "foo";
        }

        public void Dispose() => Dispose(true);

        private void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            if (disposing)
            {
                this.httpClient.Dispose();
            }

            this.disposed = true;
        }
    }
}
