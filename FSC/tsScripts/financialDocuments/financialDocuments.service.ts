import { Injectable } from '@angular/core';
import { Http, Response, Headers, RequestOptions } from '@angular/http';
import { Observable } from 'rxjs/Rx';


export class invoiceDocument {
    constructor(public Year: number, public Quarter: number, public Sum: number) {
    }
}

@Injectable()
export class FinancialDocumentsService {
    constructor(private _http: Http) {  

    }
    getDocuments() {
        return this._http.get(this.address, this.requestOptions())
            .map((response: Response) => <invoiceDocument[]>response.json())
            .catch(this.hendleError);
    }       

    address: string = "api/finances/financialDocuments/";
    requestOptions(): RequestOptions {
        let headers = new Headers({ 'Content-Type': 'application/json' });
        return new RequestOptions({ headers: headers });
    }
    hendleError(error: Response) {
        console.log(error);
        return Observable.throw(error.json().error || 'Błąd pobierania!');
    }
}
