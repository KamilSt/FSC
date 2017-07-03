export class salaryCalculatorVM {
    public salary: number;
    public salaryFrom: string;
    public typeOfContract: string;
    public higherCostOfGettingIncome: boolean;
    public healthInsurance: number;
    public accidentInsurance: number;
    public workAtLiving: boolean;
}
export class salaryCalculatorResult {
    public GrossSalary: number;
    public NetSalary: number;
    public SalaryCosts: SalaryCost[]

    public salaryCalculatorResult() {
        this.SalaryCosts = new Array<SalaryCost>();
    }
}
export class SalaryCost {
    public CostName: string;
    public CostValue: number;
    public CostPercent: number;
}