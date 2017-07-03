using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSC.ViewModels.Api
{
    public class FormSalaryCalculatorVM
    {
        public decimal salary { get; set; }
        public string salaryFrom { get; set; }
        public string typeOfContract { get; set; }
        public bool higherCostOfGettingIncome { get; set; }
        public bool healthInsurance { get; set; }
        public decimal accidentInsurance { get; set; }
        public bool workAtLiving { get; set; }
    }
}