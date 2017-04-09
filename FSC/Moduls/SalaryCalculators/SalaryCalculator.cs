using System.Collections.Generic;
using FSC.Moduls.SalaryCalculators.Interface;

namespace FSC.Moduls.SalaryCalculators
{
    public class SalaryCalculator
    {
        private IContractType EmplorSalary;
        public SalaryCalculator(IContractType emplorSalry)
        {
            EmplorSalary = emplorSalry;
        }
        public SalaryCalculatorResult Calculator()
        {
            return EmplorSalary.Calculate(EmplorSalary);
        }
    }
}
