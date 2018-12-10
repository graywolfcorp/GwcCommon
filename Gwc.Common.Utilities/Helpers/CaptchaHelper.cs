using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;

namespace Gwc.Common.Utilities.Helpers
{
    public static class CaptchaHelper
    {
        public static CaptchaResponse GetCaptchaResponse(string secret, string response, string remoteIp)
        {
            var client = new WebClient();
            var reply = client.DownloadString(
                string.Format(
                    "https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}&remoteip={2}",
                    secret,
                    response,
                    remoteIp)
                );

            return JsonConvert.DeserializeObject<CaptchaResponse>(reply);
        }

        public class CaptchaResponse
        {
            [JsonProperty("success")]
            public bool Success { get; set; }

            [JsonProperty("error-codes")]
            public List<string> ErrorCodes { get; set; }
        }
    }
}
