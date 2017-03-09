using System.Collections.Generic;
using System.Threading.Tasks;

namespace FSC.Moduls.ExchangeRates
{
    public class NBPService : INBPService
    {
        private readonly INBPClient _NBPkClient;
        public NBPService(INBPClient nbpClient)
        {
            _NBPkClient = nbpClient;
        }

        public async Task<NBPData> GetExchangeRate(string currency, string date)
        {
            var result = await _NBPkClient.GetAsync<dynamic>(currency, date);
            return readDataFromNBP(result);
        }
        public async Task<NBPData> GetExchangeRateFromLast10Days(string currency)
        {
            var result = await _NBPkClient.GetAsync<dynamic>(currency, "last/10");
            return readDataFromNBP(result);
        }
        public async Task<NBPData> GetExchangeRateFromDays(string currency, string dateFrom, string dateTo)
        {
            var result = await _NBPkClient.GetAsync<dynamic>(currency, dateFrom + "/" + dateTo);
            return readDataFromNBP(result);
        }

        private NBPData readDataFromNBP(dynamic nbpData)
        {
            var rates = new List<NBPDataRate>();
            foreach (var item in nbpData["rates"])
            {
                rates.Add(new NBPDataRate
                {
                    No = item["no"],
                    Mid = item["mid"],
                    EffectiveDate = item["effectiveDate"],
                });
            }

            return new NBPData
            {
                Table = nbpData["table"],
                Currency = nbpData["currency"],
                Code = nbpData["code"],
                Rates = rates
            };
        }
    }
}