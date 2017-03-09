using System.Threading.Tasks;

namespace FSC.Moduls.ExchangeRates
{
    public interface INBPService
    {
        Task<NBPData> GetExchangeRate(string currency, string date);
        Task<NBPData> GetExchangeRateFromLast10Days(string currency);
        Task<NBPData> GetExchangeRateFromDays(string currency, string dateFrom, string dateTo);
    }
}