using FSC.Moduls.SalaryCalculators;
using FSC.Moduls.SalaryCalculators.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tests.Moduls.SalaryCalculators
{
    public class TestyUmowaZlecenie
    {
        [Fact]
        public void Obliczenia_dla_wynagrodzenie_2000_brutto()
        {
            //Arrange
            var contract = new ContractBuilder()
            .TypeOfContract(TypeOfContract.UmowaZlecenie)
            .Salary(2000, SalaryFrom.Gross)
            .Build();
            //Act
            var calculator = new SalaryCalculator(contract);
            var result = calculator.Calculator();
            //Assert
            Assert.Equal(1712, result.NetSalary);
            Assert.Equal(2000, result.GrossSalary);
        }

        [Fact]
        public void Obliczenia_dla_wynagrodzenie_2000_brutto_z_skladka_zdrowotna()
        {
            //Arrange
            var contract = new ContractBuilder()
                .TypeOfContract(TypeOfContract.UmowaZlecenie)
                .Salary(2000, SalaryFrom.Gross)
                .HealthInsurance()
                .Build();
            //Act
            var calculator = new SalaryCalculator(contract);
            var result = calculator.Calculator();
            //Assert
            Assert.Equal(1687, result.NetSalary);
            Assert.Equal(2000, result.GrossSalary);
            Assert.Equal(133, result.SalaryCosts.First(x => x.CostName == "zaliczka na PIT").CostValue);
            Assert.Equal(180, result.SalaryCosts.First(x => x.CostName == "ubezpieczenie zdrowotne").CostValue);
        }

        [Fact]
        public void Obliczenia_dla_wynagrodzenie_2000_brutto_z_50_procentowym_kosztem_uzyskania_dochodu()
        {
            //Arrange
            var contract = new ContractBuilder()
            .TypeOfContract(TypeOfContract.UmowaZlecenie)
            .Salary(2000, SalaryFrom.Gross)
            .HigherCostOfGettingIncome(true)
            .Build();
            //Act
            var calculator = new SalaryCalculator(contract);
            var result = calculator.Calculator();
            //Assert
            Assert.Equal(1820, result.NetSalary);
            Assert.Equal(2000, result.GrossSalary);
            Assert.Equal(180, result.SalaryCosts.First(x => x.CostName == "zaliczka na PIT").CostValue);
        }

        [Fact]
        public void Obliczenia_dla_wynagrodzenie_2000_brutto_50_koszty_dochodu_oraz_skladtka_zdrowotna()
        {
            //Arrange
            var contractBuilder = new ContractBuilder()
            .TypeOfContract(TypeOfContract.UmowaZlecenie)
            .Salary(2000, SalaryFrom.Gross)
            .HigherCostOfGettingIncome(true)
            .HealthInsurance()
            .Build();
            //Act
            var calculator = new SalaryCalculator(contractBuilder);
            var result = calculator.Calculator();
            //Assert
            Assert.Equal(1795, result.NetSalary);
            Assert.Equal(2000, result.GrossSalary);
            Assert.Equal(25, result.SalaryCosts.First(x => x.CostName == "zaliczka na PIT").CostValue);
            Assert.Equal(180, result.SalaryCosts.First(x => x.CostName == "ubezpieczenie zdrowotne").CostValue);
        }

        [Theory]
        [InlineData(2000, false, false, 2336.45, 336, 0)]
        [InlineData(2000, true, false, 2197.80, 198, 0)]
        [InlineData(2000, false, true, 2371.07, 158, 213.40)]
        [InlineData(2000, true, true, 2228.41, 28, 200.56)]
        public void Obliczenia_dla_wynagrodzenie_netto
            (decimal salary, bool higherCostOfGettingIncome, bool hasHealthInsurance, decimal exceptetBrutto, decimal exceptetPit, decimal exceptetInsurance)
        {
            //Arrange
            var contractBuilder = new ContractBuilder();
            contractBuilder.TypeOfContract(TypeOfContract.UmowaZlecenie);
            contractBuilder.Salary(salary, SalaryFrom.Net);
            if (higherCostOfGettingIncome)
                contractBuilder.HigherCostOfGettingIncome(true);
            if (hasHealthInsurance)
                contractBuilder.HealthInsurance();
            var contract = contractBuilder.Build();

            //Act
            var calculator = new SalaryCalculator(contract);
            var result = calculator.Calculator();
            //Assert
            Assert.Equal(exceptetBrutto, result.GrossSalary);
            Assert.Equal(exceptetPit, result.SalaryCosts.First(x => x.CostName == "zaliczka na PIT").CostValue);
            if (hasHealthInsurance)
                Assert.Equal(exceptetInsurance, result.SalaryCosts.First(x => x.CostName == "ubezpieczenie zdrowotne").CostValue);
        }
    }
}
