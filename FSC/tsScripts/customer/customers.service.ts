import { Injectable } from '@angular/core';
import { Http, Response, Headers, RequestOptions } from '@angular/http';
import { Observable } from 'rxjs/Rx';


export class customersVM {
    CustomerId: number
    CompanyName: string
    NIP: string
    AccountNumber: string
    Address: string
    City: string
    Phone: string
}

@Injectable()
export class CustomersService {
    constructor(private _http: Http) { }

    getCutomers() {
        return this._http.get(this.address + 'Get', this.requestOptions())
            .map((response: Response) => <customersVM[]>response.json())
            .catch(this.hendleError);
    }

    getCutomer(id: number) {
        return this._http.get(this.address + 'Get/' + id)
            .map((response: Response) => <customersVM>response.json())
            .catch(this.hendleError);
    }

    addCustomer(customer: customersVM) {
        return this._http.post(this.address, customer, this.requestOptions())
            .map((response: Response) => response.json())
            .catch(this.hendleError);
    }

    updateCustomer(customer: customersVM) {
        let body = JSON.stringify(customer);
        return this._http.put(this.address + customer.CustomerId, body, this.requestOptions())
            .map((response: Response) => response.json())
            .catch(this.hendleError);
    }

    address: string = "api/Customers/";
    requestOptions(): RequestOptions {
        let headers = new Headers({ 'Content-Type': 'application/json' });
        return new RequestOptions({ headers: headers });
    }
    hendleError(error: Response) {
        console.log(error);
        return Observable.throw(error.json().error || 'Błąd pobierania!');
    }
}
