using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text.Json.Serialization;

namespace MP.Infrastructure.Helper
{
    /// <summary>
    /// Ip Helper.
    /// Work with jsonapi.
    /// https://getjsonip.com/#docs
    /// </summary>
    [Serializable]
    public static class IpHelper
    {
        public static string GetIpAddress()
        {
            HttpClient client = new();
            string result = client.GetStringAsync("https://jsonip.com/").Result;
            RemoteIpDto remoteIpDto = JsonConvert.DeserializeObject<RemoteIpDto>(result);
            return remoteIpDto.IP;
        }

        private class RemoteIpDto
        {
            [JsonPropertyName("ip")]
            public string IP { get; set; }

            [JsonPropertyName("geo-ip")]
            public string GeoIp { get; set; }

            [JsonPropertyName("API Help")]
            public string ApiHelp { get; set; }
        }
    }
}