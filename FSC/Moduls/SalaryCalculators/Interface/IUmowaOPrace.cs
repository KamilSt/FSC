namespace FSC.Moduls.SalaryCalculators.Interface
{
    public interface IUmowaOPrace : IContractType, IAccidentInsurance
    {
        bool WorkAtLiving { get; set; }
    }
}