using System;
using System.Threading.Tasks;

namespace FSC.Moduls.ExchangeRates
{
    public interface INBPClient
    {
        Task<T> GetAsync<T>(string currency, string data);
    }
}