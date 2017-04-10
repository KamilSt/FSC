using FSC.Moduls.SalaryCalculators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FSC.Moduls.SalaryCalculators.Interface;

namespace Tests.Moduls.SalaryCalculators
{
    public class TestyUmowaODzielo
    {
        [Fact]
        public void Obliczenia_dla_wynagrodzenie_2000_brutto()
        {
            //Arrange
            var contractBuilder = new ContractBuilder();
            contractBuilder.TypeOfContract(TypeOfContract.UmowaODzielo);
            contractBuilder.Salary(2000, SalaryFrom.Gross);
            contractBuilder.HigherCostOfGettingIncome(false);
            //Act
            var contract = contractBuilder.Build();
            var calculator = new SalaryCalculator(contract);
            var result = calculator.Calculator();
            //Assert
            Assert.Equal(1712, result.NetSalary);
            Assert.Equal(2000, result.GrossSalary);
        }

        [Fact]
        public void Obliczenia_dla_wynagrodzenie_2000_netto()
        {
            //Arrange
            var contract = new ContractBuilder()
                .TypeOfContract(TypeOfContract.UmowaODzielo)
                .Salary(2000, SalaryFrom.Net)
                .HigherCostOfGettingIncome(false)
                .Build();
            //Act
            var calculator = new SalaryCalculator(contract);
            var result = calculator.Calculator();
            //Assert
            Assert.Equal(2000, result.NetSalary);
            Assert.Equal(2336.45M, result.GrossSalary);
        }

        [Fact]
        public void Obliczenia_dla_wynagrodzenie_2000_netto_z_Zaliczka_na_PIT()
        {
            var contract = new ContractBuilder()
                .TypeOfContract(TypeOfContract.UmowaODzielo)
                .Salary(2000, SalaryFrom.Net)
                .HigherCostOfGettingIncome(false)
                .Build();

            var calculator = new SalaryCalculator(contract);
            var result = calculator.Calculator();

            Assert.Equal(2000, result.NetSalary);
            Assert.Equal(2336.45M, result.GrossSalary);
            Assert.Equal("zaliczka na PIT", result.SalaryCosts.First().CostName);
            Assert.Equal(360, result.SalaryCosts.First().CostValue);
        }

        [Theory]
        [InlineData(2000, SalaryFrom.Gross, 1712)]
        [InlineData(3000, SalaryFrom.Gross, 2568)]
        [InlineData(2568, SalaryFrom.Net, 3000)]
        public void Sprawdzenie_Kalkulatora_dla_kilku_wariantow_wynagrodzenia
            (decimal _salary, SalaryFrom _salaryFrom, decimal ExceptetResult)
        {
            //Arrange
            var contract = new ContractBuilder()
                .TypeOfContract(TypeOfContract.UmowaODzielo)
                .Salary(_salary, _salaryFrom)
                .HigherCostOfGettingIncome(false)
                .Build();
            //Act
            var calculator = new SalaryCalculator(contract);
            var result = calculator.Calculator();
            //Assert
            if (_salaryFrom == SalaryFrom.Gross)
                Assert.Equal(ExceptetResult, result.NetSalary);
            if (_salaryFrom == SalaryFrom.Net)
                Assert.Equal(ExceptetResult, result.GrossSalary);
        }

        [Theory]
        [InlineData(2000, true, 2197.80)]
        [InlineData(3000, true, 3296.70)]
        public void Sprawdznie_Kalkulatora_dla_kilku_wariantow_wynagrodzenia_z_50_procentem_uzyskania_dochodu
            (decimal _salary, bool _higherCost, decimal ExceptetResult)
        {
            //Arrange
            var contract = new ContractBuilder()
                .TypeOfContract(TypeOfContract.UmowaODzielo)
                .Salary(_salary, SalaryFrom.Net)
                .HigherCostOfGettingIncome(_higherCost)
                .Build();
            //Act
            var calculator = new SalaryCalculator(contract);
            var result = calculator.Calculator();
            //Assert
            Assert.Equal(ExceptetResult, result.GrossSalary);
        }
    }
}
