import { Component, Input, Output, EventEmitter } from '@angular/core';
import { FormGroup } from '@angular/forms';

import { QuestionBase } from './question-base';

@Component({
    selector: 'app-question',
    templateUrl: './tsScripts/components/filters/dynamicForm/dynamic-form-question.component.html',
    styleUrls: ["./tsScripts/components/filters/filter.component.css"],
})
export class DynamicFormQuestionComponent {
    @Input() question: QuestionBase<any>;
    @Input() form: FormGroup;
    @Output() hideElement = new EventEmitter<any>();

    questionValueChange(key, opt) {
        key.value = opt;
    }
    hideFilter(filter) {
        filter.visible = false;
        this.hideElement.emit();
    }

    get isValid() {
        return this.form.controls[this.question.key].valid;
    }
}
