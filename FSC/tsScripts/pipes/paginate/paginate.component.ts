import { Component, Input, Output, EventEmitter } from "@angular/core";

@Component({
    selector: "paginate-component",
    templateUrl: "./tsScripts/pipes/paginate/paginate.component.html"
})
export class PaginateComponent {
    @Output()
    pageChanged = new EventEmitter<number>();
    @Input()
    totalItems: any;
    @Input()
    pageSize: number;
    pager: any = {};

    constructor() { }

    ngOnChanges() {
        this.setPage(1);
    }

    setPage(page: number) {
        if (page < 1 || page > this.pager.totalPages) {
            return;
        }
        this.totalItems = Number.parseInt(this.totalItems);
        this.pager = this.getPager(this.totalItems, page, this.pageSize);
        this.pageChanged.emit(page);
    }

    getPager(totalItems: number, currentPage: number = 1, pageSize: number = 10) {
        let totalPages = Math.ceil(totalItems / pageSize);

        let startPage: number, endPage: number;
        if (totalPages <= 10) {
            // less than 10 total pages so show all
            startPage = 1;
            endPage = totalPages;
        } else {
            if (currentPage <= 6) {
                startPage = 1;
                endPage = 10;
            } else if (currentPage + 4 >= totalPages) {
                startPage = totalPages - 9;
                endPage = totalPages;
            } else {
                startPage = currentPage - 5;
                endPage = currentPage + 4;
            }
        }

        let startIndex = (currentPage - 1) * pageSize;
        let endIndex = Math.min(startIndex + pageSize - 1, totalItems - 1);
        let pages = new Array();
        for (var i = startPage; i < endPage + 1; i++)
            pages.push(i);

        return {
            totalItems: totalItems,
            currentPage: currentPage,
            pageSize: pageSize,
            totalPages: totalPages,
            startPage: startPage,
            endPage: endPage,
            startIndex: startIndex,
            endIndex: endIndex,
            pages: pages
        };
    }
}
