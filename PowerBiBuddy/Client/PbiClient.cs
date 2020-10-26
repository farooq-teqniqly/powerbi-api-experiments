using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PowerBiBuddy.Client.Models;

namespace PowerBiBuddy
{
    public class PbiClient : IDisposable
    {
        private readonly string baseAddress = "https://api.powerbi.com/v1.0/myorg/";

        private bool disposed = false;
        private readonly string authToken;
        private readonly HttpWebRequestFactory webRequestFactory;

        public PbiClient(string userAgent, string authToken, HttpWebRequestFactory webRequestFactory)
        {
            this.authToken = authToken;
            this.webRequestFactory = webRequestFactory;
        }

        public async Task<string> AddWorkspaceAsync(string workspaceJson)
        {
            var request = this.webRequestFactory.CreatePostJsonWebRequest(
                $"https://api.powerbi.com/v1.0/myorg/groups",
                workspaceJson,
                new Dictionary<string, string> { { "Authorization", this.authToken } });

            var response = (HttpWebResponse)request.GetResponse();

            string content = string.Empty;

            using (response)
            {
                var reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                content = await reader.ReadToEndAsync();
            }

            return content;
        }

        public async Task<string> AddDatasetAsync(Guid workspaceId, string datasetJson)
        {
            var request = this.webRequestFactory.CreatePostJsonWebRequest(
                $"https://api.powerbi.com/v1.0/myorg/groups/{workspaceId:D}/datasets",
                datasetJson,
                new Dictionary<string, string> {{"Authorization", this.authToken}});

            var response = (HttpWebResponse)request.GetResponse();

            string content = string.Empty;

            using (response)
            {
                var reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                content = await reader.ReadToEndAsync();
            }

            return content;
        }

        public async Task<string> AddDataToDatasetAsync(Guid workspaceId, Guid datasetId, string tableName, string dataJson)
        {
            var request = this.webRequestFactory.CreatePostJsonWebRequest(
                $"https://api.powerbi.com/v1.0/myorg/groups/{workspaceId:D}/datasets/{datasetId:D}/tables/{tableName}/rows",
                dataJson,
                new Dictionary<string, string> { { "Authorization", this.authToken } });

            var response = (HttpWebResponse)request.GetResponse();

            string content = string.Empty;

            using (response)
            {
                var reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                content = await reader.ReadToEndAsync();
            }

            return content;
        }

        public async Task<string> DeleteDataFromDatasetAsync(Guid workspaceId, Guid datasetId, string tableName)
        {
            var request = this.webRequestFactory.CreateDeleteWebRequest(
                $"https://api.powerbi.com/v1.0/myorg/groups/{workspaceId:D}/datasets/{datasetId:D}/tables/{tableName}/rows",
                new Dictionary<string, string> { { "Authorization", this.authToken } });

            var response = (HttpWebResponse)request.GetResponse();

            string content = string.Empty;

            using (response)
            {
                var reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                content = await reader.ReadToEndAsync();
            }

            return content;
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
                
            }

            this.disposed = true;
        }

        
    }
}
