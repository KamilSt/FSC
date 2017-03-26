import { Injectable } from '@angular/core';
import { Http, Response, Headers, RequestOptions } from '@angular/http'
import { Observable } from 'rxjs/Rx';


export class Checklist {
    constructor(public Id: number, public Description: string, public IsCompleted: boolean) {
    }
}

export class CheckListDisplayVM {
    constructor(public Id: number, public Description: string, public Items: Checklist[]) {
    }
}

@Injectable()
export class ChecklistsService {
    constructor(private _http: Http) {

    }

    getCheckLists() {
        return this._http.get(this.address + 'Get', this.requestOptions())
            .map((response: Response) => <Checklist[]>response.json())
            .catch(this.hendleError);
    }

    getCheckList(id: number) {
        return this._http.get(this.address + 'Get/' + id)
            .map((response: Response) => <CheckListDisplayVM>response.json())
            .catch(this.hendleError);
    }

    deleteChecklist(Id: number) {
        return this._http.delete(this.address  + Id)
            .map((response: Response) => response.json())
            .catch(this.hendleError);
    }

    createChecklist(list: Checklist) {
        let body = JSON.stringify(list);
        return this._http.post(this.address , body, this.requestOptions())
            .map((response: Response) => <Checklist>response.json())
            .catch(this.hendleError);
    }

    updateChecklist(list: CheckListDisplayVM) {
        let body = JSON.stringify(list);
        return this._http.put(this.address  + list.Id, body, this.requestOptions())
            .map((response: Response) => response.json())
            .catch(this.hendleError);
    }

    address: string = "api/CheckList/";
    requestOptions(): RequestOptions {
        let headers = new Headers({ 'Content-Type': 'application/json' });
        return new RequestOptions({ headers: headers });
    }
    hendleError(error: Response) {
        console.log(error);
        return Observable.throw(error.json().error || 'Błąd pobierania!');
    }

}
