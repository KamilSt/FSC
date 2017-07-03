using FSC.Moduls.SalaryCalculators.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSC.Moduls.SalaryCalculators
{
    public class ContractBuilder
    {
        private IContractType result;
        public ContractBuilder() { }

        public ContractBuilder Salary(decimal salary, SalaryFrom salaryFrom)
        {
            result.Salary = salary;
            result.SalaryFrom = salaryFrom;
            return this;
        }
        public ContractBuilder Salary(decimal salary, string salaryFrom)
        {
            result.Salary = salary;
            if (salaryFrom == "gross")
                result.SalaryFrom = SalaryFrom.Gross;
            if (salaryFrom == "net")
                result.SalaryFrom = SalaryFrom.Net;
            if (salaryFrom == "employerCosts")
                result.SalaryFrom = SalaryFrom.EmployerCosts;
            return this;
        }

        public ContractBuilder TypeOfContract(TypeOfContract contract)
        {
            if (SalaryCalculators.TypeOfContract.UmowaZlecenie == contract)
                result = new UmowaZlecenie();
            else if (SalaryCalculators.TypeOfContract.UmowaODzielo == contract)
                result = new UmowaODzielo();
            return this;
        }
        public ContractBuilder TypeOfContract(string contract)
        {
            //if (contract == "uop")
            //    result = ;
            if (contract == "uz")
                result = new UmowaZlecenie();
            else if (contract == "uod")
                result = new UmowaODzielo();
            return this;
        }

        public ContractBuilder HigherCostOfGettingIncome(bool higherCostOfGettingIncome)
        {
            var cost = 20;
            if (higherCostOfGettingIncome)
                cost = 50;
            if (result is IUmowaODzielo)
                ((IUmowaODzielo)result).HigherCostOfGettingIncome = cost;
            if (result is IUmowaZlecenie)
                ((IUmowaZlecenie)result).HigherCostOfGettingIncome = cost;
            return this;
        }
        public ContractBuilder HealthInsurance(bool healthInsurance = true)
        {
            if (result is IUmowaZlecenie)
                ((IUmowaZlecenie)result).HealthInsurance = healthInsurance;
            return this;
        }

        public IContractType Build()
        {
            return result;
        }

    }
    public enum TypeOfContract
    {
        UmowaOPrace,
        UmowaZlecenie,
        UmowaODzielo
    }
}