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
    constructor(private _financialServise: FinancialDocumentsService) {
    }

    ngOnInit() {
        this.showDocuments();
    }

    showDocuments() {
        this._financialServise.getDocuments().subscribe(docs => {
            this.documents = docs
        });
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