import { Injectable } from '@angular/core';
import { Http, Response, Headers, RequestOptions } from '@angular/http';
import { Observable } from 'rxjs/Rx';
import { Order } from "../orders/order.model";
import { salaryCalculatorVM, salaryCalculatorResult, SalaryCost } from "../salaryCalculator/salaryCalculator.model";

@Injectable()
export class SalaryService {
    constructor(private _http: Http) { }

    calculate(vm: salaryCalculatorVM) {
        let body = JSON.stringify(vm);
        return this._http.post(this.address, body, this.requestOptions())
            .map((response: Response) => <salaryCalculatorResult>response.json())
            .catch(this.hendleError);
    }

    address: string = "api/finances/salaryCalculator/";
    requestOptions(): RequestOptions {
        let headers = new Headers({ 'Content-Type': 'application/json' });
        return new RequestOptions({ headers: headers });
    }
    hendleError(error: Response) {
        console.log(error);
        return Observable.throw(error.json().error || 'Błąd pobierania!');
    }
}
