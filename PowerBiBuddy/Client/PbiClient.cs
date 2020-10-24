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
        private bool disposed = false;
        private readonly string authToken;
        private readonly string baseAddress = "https://api.powerbi.com/v1.0/myorg/";

        public PbiClient(string userAgent, string authToken)
        {
            this.authToken = authToken;
        }

        
        public async Task<Dataset> AddDatasetAsync(Guid workspaceId, dynamic datasetDefinition)
        {
            //string json = JsonConvert.SerializeObject(datasetDefinition);
            //var response = await this.httpClient.PostAsync($"groups/{workspaceId:D}/datasets?defaultRetentionPolicy=basicFIFO", new StringContent(json, Encoding.UTF8, "application/json"));
            //response.EnsureSuccessStatusCode();

            //return null;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"https://api.powerbi.com/v1.0/myorg/groups/{workspaceId:D}/datasets");
            request.KeepAlive = true;
            request.Method = "POST";
            request.ContentLength = 0;
            request.ContentType = "application/json";
            request.Headers.Add("Authorization", this.authToken);

            string datasetJson = "{\"name\": \"ReportFromApis_FromApp\", \"tables\": " +
                                 "[{\"name\": \"connections\", \"columns\": " +
                                 "[{ \"name\": \"appRegistrationId\", \"dataType\": \"string\"}, " +
                                 "{ \"name\": \"connectionString\", \"dataType\": \"string\"}, " +
                                 "{ \"name\": \"id\", \"dataType\": \"string\"}," +
                                 "{ \"name\": \"name\", \"dataType\": \"string\"}," +
                                 "{ \"name\": \"tenantId\", \"dataType\": \"string\"}," +
                                 "{ \"name\": \"type\", \"dataType\": \"string\"}" +
                                 "]}]}";

            byte[] byteArray = Encoding.UTF8.GetBytes(datasetJson);
            request.ContentLength = byteArray.Length;

            using (var writer = request.GetRequestStream())
            {
                writer.Write(byteArray, 0, byteArray.Length);
                
            }

            var response = (HttpWebResponse)request.GetResponse();

            string content = string.Empty;

            using (response)
            {
                var reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                content = reader.ReadToEnd();
            }

            return null;
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
