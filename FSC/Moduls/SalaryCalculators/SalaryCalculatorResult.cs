using System.Collections.Generic;

namespace FSC.Moduls.SalaryCalculators
{
    public class SalaryCalculatorResult
    {
        public decimal GrossSalary { get; set; }
        public decimal NetSalary { get; set; }
        public List<SalaryCost> SalaryCosts { get; set; }

        public SalaryCalculatorResult()
        {
            SalaryCosts = new List<SalaryCost>();
        }
    }
    public class SalaryCost
    {
        public string CostName { get; set; }
        public decimal CostValue { get; set; }
        public decimal CostPercent { get; set; }
    }
}