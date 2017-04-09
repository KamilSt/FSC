namespace FSC.Moduls.SalaryCalculators.Interface
{
    public interface IContractType
    {
        decimal Salary { get; set; }
        SalaryFrom SalaryFrom { get; set; }
        SalaryCalculatorResult Calculate(IContractType ContractType);
    }

    public enum SalaryFrom
    {
        Net,
        Gross,
        EmployerCosts,
    }
}