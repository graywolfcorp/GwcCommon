using Newtonsoft.Json;
using System;
using System.Net.Http;
using static Gwc.Common.HttpClient.HttpEnum;

namespace Gwc.Common.HttpClient
{
    public static class HttpResponseExtensions
    {
        public static T FormatContent<T>(this HttpResponseMessage response, ContentFormat returnType)
        {
            try
            {
                if (returnType == ContentFormat.HttpResponseMessage)
                {
                    return (T)(object)response;
                }
                var data = response.Content.ReadAsStringAsync().Result;
                if (returnType == ContentFormat.String)
                {
                    return (T)(object)data;
                }
                else
                {
                    return string.IsNullOrEmpty(data) ? default(T) : JsonConvert.DeserializeObject<T>(data);
                }
            }
            catch (Exception ex)
            {
                if (response.IsSuccessStatusCode)
                {
                    throw new Exception(ex.GetBaseException().Message, ex);
                }
                else
                {
                    throw new Exception(response.ReasonPhrase, ex);
                }
            }
        }
    }
}