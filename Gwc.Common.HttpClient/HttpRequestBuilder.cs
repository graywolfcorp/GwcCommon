using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Gwc.Common.HttpClient
{
    public class HttpRequestBuilder
    {
        private HttpMethod method = null;
        private string requestUri = "";
        private HttpContent content = null;
        private TimeSpan timeout = new TimeSpan(0, 0, 15);
        private bool allowAutoRedirect = false;
        private List<KeyValuePair<string, string>> headers ;

        public HttpRequestBuilder()
        {
        }

        public HttpRequestBuilder AddMethod(HttpMethod method)
        {
            this.method = method;
            return this;
        }
        
        public HttpRequestBuilder AddRequestUri(string requestUri)
        {
            this.requestUri = requestUri;
            return this;
        }

        public HttpRequestBuilder AddContent(HttpContent content)
        {
            this.content = content;
            return this;
        }

        public HttpRequestBuilder AddTimeout(TimeSpan timeout)
        {
            this.timeout = timeout;
            return this;
        }

        public HttpRequestBuilder AddAllowAutoRedirect(bool allowAutoRedirect)
        {
            this.allowAutoRedirect = allowAutoRedirect;
            return this;
        }

        public HttpRequestBuilder AddHeaders(List<KeyValuePair<string, string>> headers)
        {
            this.headers = headers;
            return this;
        }

        public async Task<HttpResponseMessage> SendAsync()
        {
            if (method == null)
                throw new ArgumentNullException("Method");

            if (string.IsNullOrEmpty(this.requestUri))
                throw new ArgumentNullException("Request Uri");

            // Set up request
            var request = new HttpRequestMessage
            {
                Method = method,
                RequestUri = new Uri(requestUri)
            };

            if (content != null)
                request.Content = content;

            foreach (var header in headers)
            {
                switch (header.Key.ToLower())
                {
                    case "accept":
                        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(header.Value));
                        break;

                    case "content-type":
                        request.Headers.TryAddWithoutValidation("Content-Type", header.Value);
                        break;

                    default:
                        request.Headers.Add(header.Key, header.Value);
                        break;
                }
            }

            // Setup client
            var handler = new HttpClientHandler();
            handler.AllowAutoRedirect = allowAutoRedirect;

            var client = new System.Net.Http.HttpClient(handler);
            client.Timeout = timeout;
            
            return await client.SendAsync(request);
        }

    }
}
