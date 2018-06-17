import { Injectable } from '@angular/core';
import { Http, Response, Headers, RequestOptions } from '@angular/http';
import { Observable } from 'rxjs/Rx';


@Injectable()
export class FiltersService {
    constructor(private _http: Http) { }

    getFilters(name) {
        let body = JSON.stringify(name);
        return this._http.post("api/Filter/Get/", body, this.requestOptions())
            .map((response: Response) => response.json())
            .catch(this.hendleError);
    }
    setFiltersStatus(filterName, $e) {
        let body = JSON.stringify($e);
        return this._http.patch("api/Filter/SaveFilter/" + filterName, body, this.requestOptions())
            .map((response: Response) => response.json())
            .catch(this.hendleError);
    }
    requestOptions(): RequestOptions {
        let headers = new Headers({ 'Content-Type': 'application/json' });
        return new RequestOptions({ headers: headers });
    }
    hendleError(error: Response) {
        console.log(error);
        return Observable.throw(error.json().error || 'Błąd pobierania!');
    }
}
