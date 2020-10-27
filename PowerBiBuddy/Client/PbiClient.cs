using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
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

        public async Task<string> AddWorkspaceAsync(AddWorkspaceRequest addWorkspaceRequest)
        {
            var request = this.webRequestFactory.CreatePostJsonWebRequest(
                $"{baseAddress}/groups",
                JsonConvert.SerializeObject(addWorkspaceRequest),
                this.authToken);

            var response = (HttpWebResponse)request.GetResponse();

            var content = string.Empty;

            using (response)
            {
                var reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                content = await reader.ReadToEndAsync();
            }

            return content;
        }

        public async Task<string> AddDatasetAsync(Guid workspaceId, AddDatasetRequest addDatasetRequest)
        {
            var request = this.webRequestFactory.CreatePostJsonWebRequest(
                $"https://api.powerbi.com/v1.0/myorg/groups/{workspaceId:D}/datasets",
                JsonConvert.SerializeObject(addDatasetRequest),
                this.authToken);

            var response = (HttpWebResponse)request.GetResponse();

            var content = string.Empty;

            using (response)
            {
                var reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                content = await reader.ReadToEndAsync();
            }

            return content;
        }

        public async Task<string> AddRowsToDatasetAsync(Guid workspaceId, Guid datasetId, string tableName, AddDatasetRowsRequest<ConnectionRow> addDatasetRowsRequest)
        {
            var request = this.webRequestFactory.CreatePostJsonWebRequest(
                $"https://api.powerbi.com/v1.0/myorg/groups/{workspaceId:D}/datasets/{datasetId:D}/tables/{tableName}/rows",
                JsonConvert.SerializeObject(addDatasetRowsRequest),
                this.authToken);

            var response = (HttpWebResponse)request.GetResponse();

            var content = string.Empty;

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
                this.authToken);

            var response = (HttpWebResponse)request.GetResponse();

            var content = string.Empty;

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
