namespace FSC.Moduls.SalaryCalculators.Interface
{
    public interface IUmowaZlecenie : IContractType, IAccidentInsurance
    {
        int HigherCostOfGettingIncome { get; set; }
        bool HealthInsurance { get; set; }
    }
}