import { Component, OnInit, Renderer } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { invoiceDocument, FinancialDocumentsService } from "../financialDocuments/financialDocuments.service";

@Component({
    selector: "financialDocuments",
    templateUrl: "./tsScripts/financialDocuments/financialDocuments.component.html",
    providers: [FinancialDocumentsService]
})
export class FinancialDocumentComponent {

    public documents: invoiceDocument[];
    constructor(private _fServise: FinancialDocumentsService) {
    }

    ngOnInit() {
        this.showDocuments();
    }

    showDocuments() {
        this._fServise.getDocuments().subscribe(x =>
            {
                this.documents = x
            });
    }
}