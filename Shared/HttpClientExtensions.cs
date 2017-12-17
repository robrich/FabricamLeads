using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Fabricam.Shared
{
    public static class HttpClientExtensions
    {

        public static HttpContent GetPayload(object Data)
        {
            if (Data != null && Data is HttpContent) {
                return (HttpContent)Data;
            } else {
                return new StringContent(JsonConvert.SerializeObject(Data), Encoding.UTF8, "application/json");
            }
        }

        public static async Task<HttpResponseMessage> PostJsonAsync(this HttpClient Client, string Url, object Data) => 
            await Client.PostAsync(Url, GetPayload(Data));

        public static async Task<HttpResponseMessage> PutJsonAsync<T>(this HttpClient Client, string Url, object Data) => 
            await Client.PutAsync(Url, GetPayload(Data));

        public static async Task<T> ReadJsonResponseAsync<T>(this HttpResponseMessage response)
        {
            string json = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(json)) {
                return default(T);
            }
            try {
                return JsonConvert.DeserializeObject<T>(json);
            } catch (Exception ex) {
                // Capture response string for better logs, FRAGILE: possible PII leak
                throw new Exception($"Can't deserialize {json} as {typeof(T).FullName}", ex);
            }
        }

    }
}
