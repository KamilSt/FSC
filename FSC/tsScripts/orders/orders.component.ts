import { Component, OnInit } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { Router, ActivatedRoute, Params } from '@angular/router';

import { OrdersService } from "../orders/orders.service";
import { FinancialDocumentsService } from "../financialDocuments/financialDocuments.service";

@Component({
    selector: "orders",
    templateUrl: "./tsScripts/orders/orders.component.html",
   // styleUrls: ["./tsScripts/orders/orders.css"],
   //  directives:[],       
    providers: [OrdersService, FinancialDocumentsService]  
})
export class OrdersComponent {
    public orderListVM ;
  
    constructor(private _orderServise: OrdersService, private _financialServise: FinancialDocumentsService, private router: Router) {
        this._orderServise.getOrders().subscribe(x =>  this.orderListVM = x );
    }
   
    editOrder(id: number) {
        this.router.navigate(['/newOrder', { Id: id }]);
    }

    deleteOrder(id: number) {
    } 
    makeInvoice(order) {
        this._orderServise.createInvoice(order.Id).subscribe(x => this.showInvoiceNumber(x, order));
    }
    showInvoiceNumber(result, order) {
        var orderToUpdate = this.orderListVM.Orders.find(x => x == order);
        orderToUpdate.Invoiced = true;
        orderToUpdate.InvoiceNumber = result.InvoiceNmuber;
    }
    downloadFile(id) {
        this._financialServise.downloadPDF(id).subscribe(
            response => {
                var header = response.headers.get('content-disposition');
                var headerFilmName = header.split(';')[1].trim().split('=')[1];
                var fileName = headerFilmName.replace(/"/g, '');
                var mediaType = 'application/octet-stream; ';
                var myBlob = new Blob([response._body], { type: mediaType })
                var blobURL = (window.URL).createObjectURL(myBlob);
                var anchor = document.createElement("a");
                anchor.download = fileName;
                anchor.href = blobURL;
                anchor.click();
            }
        );
    }
}
