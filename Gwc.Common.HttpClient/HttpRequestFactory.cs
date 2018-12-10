using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static Gwc.Common.HttpClient.HttpEnum;

namespace Gwc.Common.HttpClient
{
    public static class HttpRequestFactory
    {
        public static async Task<T> DeleteAsync<T>(string requestUri, List<KeyValuePair<string, string>> headers, 
            ContentFormat responseContentFormat = ContentFormat.Type)
        {
            var builder = new HttpRequestBuilder()
                                .AddMethod(HttpMethod.Delete)
                                .AddRequestUri(requestUri)
                                .AddHeaders(headers);

            var response = await builder.SendAsync();
            return response.FormatContent<T>(responseContentFormat);
        }
        public static async Task<string> DeleteAsync(string requestUri, 
            List<KeyValuePair<string, string>> headers)
            => await DeleteAsync<string>(requestUri, headers, ContentFormat.String);
        public static async Task<T> GetAsync<T>(string requestUri, List<KeyValuePair<string, string>> headers, 
            ContentFormat responseContentFormat = ContentFormat.Type)
        {
            var builder = new HttpRequestBuilder()
                                .AddMethod(HttpMethod.Get)
                                .AddRequestUri(requestUri)
                                .AddHeaders(headers);

            var response = await builder.SendAsync();
            return response.FormatContent<T>(responseContentFormat);
        }
        public static async Task<string> GetAsync(string requestUri, 
            List<KeyValuePair<string, string>> headers)
            => await GetAsync<string>(requestUri, headers, ContentFormat.String);
        public static async Task<T> PatchAsync<T>(string requestUri, object value,
            List<KeyValuePair<string, string>> headers, ContentFormat responseContentFormat = ContentFormat.Type)
        {
            var builder = new HttpRequestBuilder()
                                .AddMethod(new HttpMethod("PATCH"))
                                .AddRequestUri(requestUri)
                                .AddContent(new PatchContent(value))
                                .AddHeaders(headers);

            var response = await builder.SendAsync();
            return response.FormatContent<T>(responseContentFormat);
        }
        public static async Task<string> PatchAsync(string requestUri, object value, 
            List<KeyValuePair<string, string>> headers)
            => await PatchAsync<string>(requestUri, value, headers, ContentFormat.String);
        public static async Task<T> PostAsync<T>(string requestUri, object value,
            List<KeyValuePair<string, string>> headers, ContentFormat responseContentFormat = ContentFormat.Type)
        {
            var builder = new HttpRequestBuilder()
                                .AddMethod(HttpMethod.Post)
                                .AddRequestUri(requestUri)
                                .AddContent(new JsonContent(value))
                                .AddHeaders(headers);

            var response = await builder.SendAsync();
            return response.FormatContent<T>(responseContentFormat);
        }
        public static async Task<string> PostAsync(string requestUri, object value, 
            List<KeyValuePair<string, string>> headers)
            => await PostAsync<string>(requestUri, value, headers, ContentFormat.String);
        public static async Task<HttpResponseMessage> PostFileAsync(string requestUri,
            string filePath, string apiParamName, List<KeyValuePair<string, string>> headers = null)
        {
            var builder = new HttpRequestBuilder()
                                .AddMethod(HttpMethod.Post)
                                .AddRequestUri(requestUri)
                                .AddContent(new FileContent(filePath, apiParamName))
                                .AddHeaders(headers);

            return await builder.SendAsync();
        }
        public static async Task<T> PutAsync<T>(string requestUri, object value,
            List<KeyValuePair<string, string>> headers, ContentFormat responseContentFormat = ContentFormat.Type)
        {
            var builder = new HttpRequestBuilder()
                                .AddMethod(HttpMethod.Put)
                                .AddRequestUri(requestUri)
                                .AddContent(new JsonContent(value))
                                .AddHeaders(headers);

            var response = await builder.SendAsync();
            return response.FormatContent<T>(responseContentFormat);
        }
        public static async Task<string> PutAsync(string requestUri, object value, 
            List<KeyValuePair<string, string>> headers)
          => await PutAsync<string>(requestUri, value, headers, ContentFormat.String);
    }
}