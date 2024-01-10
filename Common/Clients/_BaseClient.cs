using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Net.Http;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Net.Http.Headers;

namespace Common.Clients
{
    public class BaseClient
    {
        public HttpClient client;
        protected JsonSerializerOptions serializerOptions;
        public string ApiUrl { get; set; }

        public BaseClient()
        {
            serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
        }
        public BaseClient(string apiUrl) : this()
        {
            ApiUrl = apiUrl;
            client = new HttpClient();
            client.DefaultRequestHeaders.Add("user-agent", "Chrome/94.0.4606.71");
            //client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
            client.DefaultRequestHeaders.Add("Connection", "keep-alive");
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }

        public async Task<T> CallApi<T>(string uri)
        {
            try
            {
                if (!string.IsNullOrEmpty(ApiUrl))
                    uri = ApiUrl + uri;
                else
                    uri = client.BaseAddress + uri;

                HttpResponseMessage response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == HttpStatusCode.NoContent)
                    {
                        return default;
                    }
                    string result = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<T>(result, serializerOptions);

                    //// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
                }
                else
                {
                    Console.WriteLine("API call failed: " + response.StatusCode + " - " + response.ReasonPhrase);
                }
                return default;
            }
            catch (Exception ex)
            {
                Console.WriteLine(@"\tERROR {0}", ex.Message);
            }
            return default;
        }
    }
}