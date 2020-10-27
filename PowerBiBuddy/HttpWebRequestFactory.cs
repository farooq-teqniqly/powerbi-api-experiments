using System.Net;
using System.Text;

namespace PowerBiBuddy
{
    public class HttpWebRequestFactory
    {
        public HttpWebRequest CreatePostJsonWebRequest(string uri, string body, string authToken)
        {
            return CreatePostRequest(uri, body, authToken);
        }

        public HttpWebRequest CreateDeleteWebRequest(string uri, string authToken)
        {
            return CreateDeleteRequest(uri, authToken);
        }

        private HttpWebRequest CreateGetRequest(string uri, string authToken)
        {
            var request = CreateRequest(uri, authToken);
            request.Method = "GET";
            return request;
        }

        private HttpWebRequest CreatePostRequest(string uri, string body, string authToken)
        {
            var request = CreateRequest(uri, authToken);
            request.Method = "POST";

            var byteArray = Encoding.UTF8.GetBytes(body);
            request.ContentLength = byteArray.Length;

            using (var writer = request.GetRequestStream())
            {
                writer.Write(byteArray, 0, byteArray.Length);

            }

            return request;
        }

        private HttpWebRequest CreateDeleteRequest(string uri, string authToken)
        {
            var request = CreateRequest(uri, authToken);
            request.Method = "DELETE";
            return request;
        }

        private HttpWebRequest CreateRequest(string uri, string authToken)
        {
            var request = (HttpWebRequest)WebRequest.Create(uri);
            request.KeepAlive = true;
            request.ContentLength = 0;
            request.ContentType = "application/json";
            request = AddAuthHeader(request, authToken);

            return request;
        }

        private HttpWebRequest AddAuthHeader(HttpWebRequest request, string authToken)
        {
            request.Headers.Add("Authorization", authToken);
            return request;
        }
    }
}
