using FSC.Moduls.SalaryCalculators.Interface;
using System;

namespace FSC.Moduls.SalaryCalculators
{
    public class UmowaODzielo : IUmowaODzielo
    {
        public decimal Salary { get; set; }
        public int HigherCostOfGettingIncome { get; set; }
        public SalaryFrom SalaryFrom { get; set; }

        public SalaryCalculatorResult Calculate(IContractType EmployerSalary)
        {
            var result = new SalaryCalculatorResult();
            var es = EmployerSalary as IUmowaODzielo;
            if (es.SalaryFrom.Equals(SalaryFrom.Gross) || es.SalaryFrom.Equals(SalaryFrom.EmployerCosts))
            {
                result.GrossSalary = es.Salary;
                var podstawaOpodatkowania = es.Salary * (1 - ((decimal)es.HigherCostOfGettingIncome / 100));
                var kosztUzyskaniaPrzychodu = es.Salary - podstawaOpodatkowania;
                var zaliczkaNaPIT = Math.Round(podstawaOpodatkowania * 0.18M, 0);
                result.NetSalary = result.GrossSalary - zaliczkaNaPIT;
                result.SalaryCosts.Add(new SalaryCost()
                {
                    CostName = "zaliczka na PIT",
                    CostValue = zaliczkaNaPIT
                });
            }
            else if (es.SalaryFrom.Equals(SalaryFrom.Net))
            {
                result.GrossSalary = Math.Round(es.Salary / 0.856M, 2);
                if (es.HigherCostOfGettingIncome == 50)
                    result.GrossSalary = Math.Round(es.Salary / 0.91M, 2);
                result.NetSalary = es.Salary;
                var podstawaOopodatkowania = (es.Salary / 0.8M) * (1 - ((decimal)es.HigherCostOfGettingIncome / 100));
                var zaliczkaNaPIT = Math.Round(podstawaOopodatkowania * 0.18M, 0);
                result.SalaryCosts.Add(new SalaryCost()
                {
                    CostName = "zaliczka na PIT",
                    CostValue = zaliczkaNaPIT
                });
            }
            return result;
        }
    }
}