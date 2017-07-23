import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { ControlValueAccessor } from '@angular/forms';
import { CustomersService, customersVM } from "../customer/customers.service";

@Component({
    selector: "customer-selector",
    providers: [CustomersService],
    styles: [`
    .customerItem {
         margin-left: 5px;}
    `],
    template: `
        <div class="btn-group">
          <button type="button" class="btn btn-default" style="height: 54px;">
                <span [innerHTML]="customer"></span>
          </button>
          <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false" style=" height: 54px;">
            <span class="caret"></span>
            <span class="sr-only">Toggle Dropdown</span>
          </button>
          <ul class="dropdown-menu">
           <li class ="customerItem" *ngFor="let customerItem of customerList" value="{{customerItem.value}}" (click)="select.emit($event.target.value)">{{customerItem.display}} </li>
          </ul>
        </div>
        `
})

export class CustomerSelector {
    @Output() select = new EventEmitter();
    @Input() customerId;
    customerList: SelectItem[];
    _customer: customersVM;
    constructor(private _customersService: CustomersService) { }

    ngOnChanges() {
        this.select.emit(this.customerId);
        this._customersService.getCustomer(this.customerId).subscribe(x => this._customer = x);
    }
    get customer() {
        if (this._customer == null)
            return "Wybierz->";
        else
            return this._customer.CompanyName + "<br/>" + this._customer.NIP;
    }
    ngOnInit() {
        this._customer = new customersVM();
        if (this.customerId !== 0) {
            this._customersService.getCustomer(this.customerId).subscribe(x => this._customer = x);
        }
        this._customersService.getCustomers().subscribe(x => this.showCustomers(x));
        this.customerList = new Array<SelectItem>();
        this.select.emit(this.customerId);
    }
    showCustomers(customers) {
        customers.forEach(x => {
            this.customerList.push(new SelectItem(x.CompanyName, x.CustomerId));
        });
    }
}
class SelectItem {
    constructor(public display: string, public value: number) { }
}