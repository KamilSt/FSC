using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSC.Moduls.ExchangeRates
{
    public class NBPData
    {
        public NBPData()
        {
            Rates = new List<NBPDataRate>();
        }

        public string Table { get; set; }
        public string Currency { get; set; }
        public string Code { get; set; }
        public List<NBPDataRate> Rates { get; set; }
    }

    public class NBPDataRate
    {
        public string No { get; set; }
        public string EffectiveDate { get; set; }
        public decimal Mid { get; set; }
    }
}