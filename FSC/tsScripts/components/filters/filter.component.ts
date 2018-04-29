import { Component, OnInit, EventEmitter, Input, Output, } from '@angular/core';
//import { FormsModule } from '@angular/forms';
//import { Router, ActivatedRoute, Params } from '@angular/router';

import { FiltersService } from "../filters/filters.service";
import { DropdownQuestion } from "./dynamicForm/question-dropdown";
import { TextboxQuestion } from "./dynamicForm/question-textbox";

@Component({
    selector: "filterComponent",
    templateUrl: "./tsScripts/components/filters/filter.component.html",
    styleUrls: ["./tsScripts/components/filters/filter.component.css"],
    providers: [FiltersService]
})
export class FilterComponent {
    questions: any[]; 
    isDataAvailable: boolean = false;
    @Output() filterQuery = new EventEmitter<any>();
    @Input() filterName: string;

    constructor(private _filtersService: FiltersService) { }
    ngOnInit() {
        this._filtersService.getFilters(this.filterName).subscribe(docs => {
            this.displayFilters(docs);
        });
    }

    serchQuery($event) {
        this.filterQuery.emit($event);
    }
    displayFilters(filtry) {
        this.questions = [];
        filtry.Filters.forEach((x, i) => {
            switch (x.controlType) {

                case 'textbox':
                    this.questions.push(
                        new TextboxQuestion({
                            key: x.key,
                            label: x.label,
                            value: x.value,
                            visible: x.visible,
                            order: x.order
                        }))
                    break;

                case 'dropdown':
                    let options: { key: string, value: string }[] = [];
                    x.options.forEach((w) => {
                        options.push({ key: w.key, value: w.label });
                    });

                    this.questions.push(new DropdownQuestion({
                        key: x.key,
                        label: x.label,
                        value: x.value,
                        visible: x.visible,
                        options: options,
                        order: x.order
                    }))
                    break;
            }
        });
        this.isDataAvailable = true;
        return this.questions.sort((a, b) => a.order - b.order);
    }
}