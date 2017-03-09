using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

namespace FSC.Moduls.ExchangeRates
{
    public class NBPClient : INBPClient
    {
        private readonly HttpClient _httpClient;
        public NBPClient()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://api.nbp.pl/api/exchangerates/")
            };
            _httpClient.DefaultRequestHeaders
                .Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<T> GetAsync<T>(string currency, string data)
        {
            var url = $"rates/a/{currency}/{data}/";
            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
                return default(T);

            var result = await response.Content.ReadAsStringAsync();
            return new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<T>(result);
        }
    }
}