import { Component, OnInit } from "@angular/core";
import { FormsModule, FormBuilder, Validators } from "@angular/forms";
import { SalaryService } from "../salaryCalculator/salaryCalculator.service";
import { salaryCalculatorVM, salaryCalculatorResult, SalaryCost } from "../salaryCalculator/salaryCalculator.model";

@Component({
    selector: "salaryCalc",
    templateUrl: "./tsScripts/salaryCalculator/salaryCalculator.component.html",
    styleUrls: ["./tsScripts/salaryCalculator/salaryCalculator.css"],
    providers: [SalaryService]
})
export class SalaryCalculatorComponent {

    private salaryVM: salaryCalculatorVM;
    private result: salaryCalculatorResult;

    constructor(private _salaryService: SalaryService) { }

    ngOnInit() {

        this.salaryVM = new salaryCalculatorVM();
        this.salaryVM.typeOfContract = "uz";
        this.salaryVM.salaryFrom = "gross";
    }

    salaryFromList = [
        { 'display': 'brutto', 'value': 'gross' },
        { 'display': 'netto', 'value': 'net' },
        { 'display': 'koszty pracodawcy', 'value': 'employerCosts' }];

    onSubmit() {
        this._salaryService.calculate(this.salaryVM).subscribe(result =>   this.result = result );
    }
}
