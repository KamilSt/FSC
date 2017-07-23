import { Component, OnInit } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { CustomersService, customersVM } from "../customer/customers.service";

@Component({
    selector: "cutomers",
    templateUrl: "./tsScripts/customer/customers.component.html",
    providers: [CustomersService]
})
export class CustomersComponent {
    customersVM: customersVM[];
    editableCustomer = {};

    constructor(private _customersService: CustomersService) {
        this.reload();
    }

    edit(customer) {
        this.editableCustomer = Object.assign({}, customer)
    }

    save(customer) {
        if (customer.CustomerId) {
            this._customersService.updateCustomer(customer).subscribe();
        } else {
            this._customersService.addCustomer(customer).subscribe();
        }
        this.reload();
        this.editableCustomer = {}
    }

    private reload() {
        this._customersService.getCustomers().subscribe(x => this.customersVM = x);
    }
}

