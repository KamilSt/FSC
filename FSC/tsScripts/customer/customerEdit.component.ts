import { Component, OnInit, EventEmitter, Output, Input } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Checklist, ChecklistVM, ChecklistsService } from "../checklist/checklists.service";

@Component({
    selector: "cutomer-edit",   
    template: `
            <div class="">
                <input type="text" [(ngModel)]="customer.CompanyName" placeholder="Nazwa firmy" style="width: 10%;">
                <input type="text" [(ngModel)]="customer.NIP" placeholder="NIP" style="width: 10%;">
                <input type="text" [(ngModel)]="customer.AccountNumber" placeholder="Numer konta" style="width: 10%;">
                <input type="text" [(ngModel)]="customer.City" placeholder="Miasto" style="width: 10%;">
                <input type="text" [(ngModel)]="customer.Address" placeholder="Adres" style="width: 10%;">
                <input type="text" [(ngModel)]="customer.Phone" placeholder="Telefon" style="width: 10%;">

                <button (click)="onSave()" class="btn">zapisz!</button>  
           </div>
`,
    providers: [ChecklistsService]
})
export class CustomerEditComponent {

    @Input() customer = {};
    @Output() save = new EventEmitter();

    onSave() {
        this.save.emit(this.customer);
    }
}