import { Injectable } from '@angular/core';
import { Http, Response, Headers, RequestOptions } from '@angular/http';
import { Observable } from 'rxjs/Rx';
import { Order } from "../orders/order.model";

@Injectable()
export class OrdersService {
    constructor(private _http: Http) { }

    getOrders() {
        return this._http.get(this.address + 'Get', this.requestOptions())
            .map((response: Response) => response.json())
            .catch(this.hendleError);
    }
    getOrder(id: number) {
        return this._http.get(this.address + 'Get/' + id, this.requestOptions())
            .map((response: Response) => <Order>response.json())
            .catch(this.hendleError);
    }
    createOrder(list) {
        let body = JSON.stringify(list);
        return this._http.post(this.address, body, this.requestOptions())
            .map((response: Response) => response.json())
            .catch(this.hendleError);
    }
    updateOrder(list) {
        let body = JSON.stringify(list);
        return this._http.put(this.address + list.Id, body, this.requestOptions())
            .map((response: Response) => <Order>response.json())
            .catch(this.hendleError);
    }

    address: string = "api/Orders/";
    requestOptions(): RequestOptions {
        let headers = new Headers({ 'Content-Type': 'application/json' });
        return new RequestOptions({ headers: headers });
    }
    hendleError(error: Response) {
        console.log(error);
        return Observable.throw(error.json().error || 'Błąd pobierania!');
    }
}
