using FSC.Moduls.SalaryCalculators.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSC.Moduls.SalaryCalculators
{
    public class UmowaZlecenie : IUmowaZlecenie
    {
        public decimal AccidentInsurance { get; set; }
        public bool HealthInsurance { get; set; }
        public int HigherCostOfGettingIncome { get; set; }
        public decimal Salary { get; set; }
        public SalaryFrom SalaryFrom { get; set; }

        public SalaryCalculatorResult Calculate(IContractType EmployerSalary)
        {
            var result = new SalaryCalculatorResult();
            var umowa = EmployerSalary as IUmowaZlecenie;
            if (umowa.SalaryFrom.Equals(SalaryFrom.Gross))
            {
                var brutto = umowa.Salary;
                var kosztUzyskaniaPrzychodu = brutto * 0.20M;
                if (umowa.HigherCostOfGettingIncome == 50)
                    kosztUzyskaniaPrzychodu = brutto * 0.50M;
                var dochodDoOpadatkowania = brutto - kosztUzyskaniaPrzychodu;
                var podatek = 0M;
                var netto = 0M;
                if (umowa.HealthInsurance == true)
                {
                    var skladkaNaubiezpiczenieZdrowotne = brutto * 0.09M;
                    var skladkaNaubiezpiczenieZdrowotnePomiejszajacaPodatek = brutto * 0.0775M;
                    
                    podatek = dochodDoOpadatkowania * 0.18M;
                    podatek = podatek - skladkaNaubiezpiczenieZdrowotnePomiejszajacaPodatek;
                    podatek = Math.Round(podatek, 0, MidpointRounding.AwayFromZero);
                    netto = brutto - skladkaNaubiezpiczenieZdrowotne - podatek;
                    result.SalaryCosts.Add(new SalaryCost()
                    {
                        CostName = "ubezpieczenie zdrowotne",
                        CostPercent = 2.45M,
                        CostValue = skladkaNaubiezpiczenieZdrowotne
                    });
                }
                else
                {
                    podatek = dochodDoOpadatkowania * 0.18M;
                    podatek = Math.Round(podatek, 0, MidpointRounding.AwayFromZero);
                    netto = brutto - podatek;
                }

                result.GrossSalary = brutto;
                result.NetSalary = netto;

                result.SalaryCosts.Add(new SalaryCost()
                {
                    CostName = "zaliczka na PIT",
                    CostValue = podatek

                });
            }
            if (umowa.SalaryFrom.Equals(SalaryFrom.Net))
            {
                if (umowa.HealthInsurance == false)
                {
                    var netto = umowa.Salary;
                    result.NetSalary = umowa.Salary;
                    result.GrossSalary = Math.Round(umowa.Salary / 0.856M, 2);
                    if (umowa.HigherCostOfGettingIncome == 50)
                        result.GrossSalary = Math.Round(umowa.Salary / 0.91M, 2);
                    var podatek = result.GrossSalary - result.NetSalary;
                    podatek = Math.Round(podatek, 0);
                    result.SalaryCosts.Add(new SalaryCost()
                    {
                        CostName = "zaliczka na PIT",
                        CostValue = podatek

                    });
                }
                if (umowa.HealthInsurance == true)
                {
                    var netto = umowa.Salary;
                    result.NetSalary = umowa.Salary;
                    result.GrossSalary = Math.Round(umowa.Salary / 0.8435M, 2);
                    if (umowa.HigherCostOfGettingIncome == 50)
                        result.GrossSalary = Math.Round(umowa.Salary / 0.8975M, 2);
                    var ubezpiecznieZdrowotne = Math.Round(result.GrossSalary * 0.09M, 2);
                    var podatek = result.GrossSalary - result.NetSalary - ubezpiecznieZdrowotne;
                    podatek = Math.Round(podatek, 0);
                    result.SalaryCosts.Add(new SalaryCost()
                    {
                        CostName = "zaliczka na PIT",
                        CostValue = podatek
                        //poprawić podatek z funkcji brutto zwracanie
                    });
                    result.SalaryCosts.Add(new SalaryCost()
                    {
                        CostName = "ubezpieczenie zdrowotne",
                        CostValue = ubezpiecznieZdrowotne

                    });
                }
            }

            return result;
        }
    }
}