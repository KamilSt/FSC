﻿import { Component, OnInit } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { OrdersService } from "../orders/orders.service";
import { Router, ActivatedRoute, Params } from '@angular/router';

@Component({
    selector: "orders",
    templateUrl: "./tsScripts/orders/orders.component.html",
   // styleUrls: ["./tsScripts/orders/orders.css"],
   //  directives:[],       
    providers: [OrdersService]  
})
export class OrdersComponent {
    public orderListVM ;
  
    constructor(private _orderServise: OrdersService, private router: Router) {

        this._orderServise.getOrders().subscribe(x =>  this.orderListVM = x );
    }
   
    editOrder(id: number) {
        this.router.navigate(['/newOrder', { Id: id }]);
    }

    deleteOrder(id: number) {

    } 
}
