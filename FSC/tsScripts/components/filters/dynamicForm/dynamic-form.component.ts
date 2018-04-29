import { Component, Input, Output, OnInit, EventEmitter } from '@angular/core';
import { FormGroup } from '@angular/forms';

import { QuestionBase } from './question-base';
import { QuestionControlService } from './question-control.service';

@Component({
    selector: 'app-dynamic-form',
    templateUrl: './tsScripts/components/filters/dynamicForm/dynamic-form.component.html',
    styleUrls: ["./tsScripts/components/filters/filter.component.css"],
    providers: [QuestionControlService]
})
export class DynamicFormComponent implements OnInit {

    @Input() questions: QuestionBase<any>[] = [];
    @Output() serchQuery = new EventEmitter<any>();
    form: FormGroup;
    hiddenFilters = [];

    constructor(private qcs: QuestionControlService) { }

    ngOnInit() {
        this.form = this.qcs.toFormGroup(this.questions);
    }
    onSubmit() {
        var formValue = this.questions.filter(x => x.visible === true);
        this.serchQuery.emit(JSON.stringify(formValue));
    }
    refreshElement() {
        this.hiddenFilters = this.questions.filter(x => x.visible === false);
    }
    showFilter(control) {
        control.visible = true;
        this.refreshElement();
    }
}
