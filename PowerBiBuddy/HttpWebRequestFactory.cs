using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PowerBiBuddy
{
    public class HttpWebRequestFactory
    {
        public HttpWebRequest CreatePostJsonWebRequest(
            string uri,
            string content,
            IDictionary<string, string> headers)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.KeepAlive = true;
            request.Method = "POST";
            request.ContentLength = 0;
            request.ContentType = "application/json";

            foreach (var header in headers)
            {
                request.Headers.Add(header.Key, header.Value);
            }

            byte[] byteArray = Encoding.UTF8.GetBytes(content);
            request.ContentLength = byteArray.Length;

            using (var writer = request.GetRequestStream())
            {
                writer.Write(byteArray, 0, byteArray.Length);

            }

            return request;
        }

        public HttpWebRequest CreateDeleteWebRequest(
            string uri,
            IDictionary<string, string> headers)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.KeepAlive = true;
            request.Method = "DELETE";

            foreach (var header in headers)
            {
                request.Headers.Add(header.Key, header.Value);
            }

            return request;
        }
    }
}
