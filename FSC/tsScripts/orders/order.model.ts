
export class newOrder {
    constructor(public Id: number, public CustomerId: number, public Description: string, public Items: newOrderItem[]) { }

    get BruttoSum(): string {
        let sum = 0;
        this.Items.forEach(x => sum += x.Brutto);
        return this.roundTo2Decimal(sum);
    }
    get VATSum(): string {
        let sum = 0;
        this.Items.forEach(x => sum += x.AmountVAT);
        return this.roundTo2Decimal(sum);
    }
    get RateSum(): string {
        let sum = 0;
        this.Items.forEach(x => sum += x.Rate);
        return this.roundTo2Decimal(sum);
    }

    private roundTo2Decimal(amount) {
        return (Math.round(amount * 100) / 100).toFixed(2);
    }
}

export class newOrderItem {

    constructor(
        public Id: number,
        public Servis: string,
        public Quantity: number,
        public Rate: number,
        public VAT: number,
        public Status: Status
    ) { };

    public EmptyValue() {
        return new newOrderItem(0, "", 0, 0, 0, Status.New);
    }

    get AmountVAT(): number {
        var vat = this.Rate * this.Quantity * (this.VAT / 100);
        return Math.round(vat * 100) / 100;
    }
    get Brutto(): number {
        var brutto = this.AmountVAT + (this.Rate * this.Quantity);
        return Math.round(brutto * 100) / 100;
    }
    set Brutto(brutto: number) {
        var amount = brutto / ((1 + (this.VAT / 100)) * this.Quantity);
        this.Rate = Math.round(amount * 100) / 100;
    }
}

export class Order {
    public Id: number;  
    CustomerId: number; 
    Description: string;
    public OrderItems: OrderItem[];

}

export class OrderItem {
    public OrderItemId: number;
    public ServiceItemCode: string;
    public ServiceItemName: string;
    public Quantity: number;
    public Rate: number;
    public VAT: number;
}

export enum Status {
    New,
    Modyficate,
    Delete,
    Orginal
}