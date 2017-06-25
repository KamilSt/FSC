import { Component, OnInit } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { OrdersService } from "../orders/orders.service";
import { newOrder, newOrderItem, Order, OrderItem, Status } from "../orders/order.model";
import { Router, ActivatedRoute, Params } from '@angular/router';

@Component({
    selector: "newOrder",
    templateUrl: "./tsScripts/orders/newOrder.component.html",     
    providers: [OrdersService]
})
export class NewOrderComponent {

    public newOder: newOrder;
    public newOrderItem: newOrderItem;
    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private _orderServise: OrdersService
    ) { }

    ngOnInit() {
        let id = this.route.snapshot.params['Id'];
        if (!Number.isNaN(id)) {
            this._orderServise.getOrder(id).subscribe(x => {
                this.showOrder(x);
            });
        }
        this.newOder = new newOrder(0, "", new Array<newOrderItem>());
        this.newOrderItem = new newOrderItem(0, "", 0, 0, 0, Status.New);

    }

    showOrder(order: Order) {
        this.newOder = new newOrder(order.Id, order.Description, new Array<newOrderItem>());
        order.OrderItems.forEach((item, i) => {
            this.newOder.Items.push(new newOrderItem(item.OrderItemId, item.ServiceItemName, item.Quantity, item.Rate, item.VAT, Status.Orginal));
        })
    }

    saveOrder() {
        var order;
        order = this.newOder;
        order.OrderItems = this.newOder.Items;
        if (order.Id === 0) {
            this._orderServise.createOrder(order).subscribe(x => this.showOrder(x));
        }
        this._orderServise.updateOrder(order).subscribe(x => this.showOrder(x));
    }

    addItem($event) {
        if (this.canAdd() && ($event.which === 1 || $event.which === 13)) {
            this.newOder.Items.push(this.newOrderItem);
            this.newOrderItem = this.newOrderItem.EmptyValue();
        }
    }

    updateOrderItem($event, item) {
        if (item !== "" && $event.which === 13) {
            if (item.Status == Status.Orginal)
                item.Status = Status.Modyficate;
            this.setEditState(item, false);
        }
    }
  
    deleteItem(item: newOrderItem) {
        if (item.Status == Status.New)
            this.newOder.Items.splice(this.newOder.Items.indexOf(item), 1);
        item.Status = Status.Delete;
    }

    setEditState(item, state) {
        if (state) {
            item.isEditMode = state;
        } else {
            delete item.isEditMode;
        }
    }

    canAdd() {
        return (this.newOrderItem.Brutto > 0 && this.newOrderItem.Servis !== '')
    }
}
