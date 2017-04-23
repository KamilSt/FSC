import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Checklist, ChecklistVM, ChecklistsService } from '../checklist/checklists.service';

@Component({
    selector: 'checklist',
    templateUrl: './tsScripts/checklist/checklist.component.html',
    styleUrls: ['./tsScripts/checklist/checkList.css'],
    // directives:[],
    providers: [ChecklistsService]
})
export class ChecklistsComponent {

    public checklists: Checklist[];
    public checklistVisibility: boolean;
    public selectedChecklist: ChecklistVM;
    public checklistToAdd: Checklist = new Checklist(0, "", false);
    public newChecklistItem: string;

    constructor(private _checklistServise: ChecklistsService) {
    }

    ngOnInit() {
        this.getChecklists();
    }
    getChecklists() {
        this._checklistServise.getCheckLists().subscribe(x => this.checklists = x);
    }
    showChecklist(Id: number) {
        this._checklistServise.getCheckList(Id).subscribe(x => this.selectedChecklist = x);
        this.checklistVisibility = true;
    }
    deselect() {
        this.checklistVisibility = false;
        this.selectedChecklist = null;
    }
    addChecklist() {
        this._checklistServise.createChecklist(this.checklistToAdd)
            .subscribe(x => this.checklistToAdd.Id = x.Id);
        this.checklistToAdd = new Checklist(0, "", false);
        this.getChecklists();
    }
    setEditState(todo, state) {
        if (state) {
            todo.isEditMode = state;
        } else {
            delete todo.isEditMode;
        }
    }
    updateStatusChecklist(checklist) {
        var _checklistItem = {
            Id: checklist.Id,
            Description: checklist.Description,
            IsCompleted: !checklist.IsCompleted,
            ParentId: this.selectedChecklist.Id
        };
        this._checklistServise.updatejeden(_checklistItem)
            .map(res => res.json())
            .subscribe(x => { checklist.isCompleted = !checklist.isCompleted; });
    }
    updateChecklistItemText($event, checklistItem) {
        if (checklistItem !== "" && $event.which === 13) {
            checklistItem.Description = $event.target.value;
            var _checklistItem = {
                Id: checklistItem.Id,
                Description: checklistItem.Description,
                IsCompleted: checklistItem.IsCompleted,
                ParentId: this.selectedChecklist.Id
            };
            this._checklistServise.updatejeden(_checklistItem)
                .map(res => res.json())
                .subscribe(data => {
                    this.setEditState(checklistItem, false);
                });
            this.setEditState(checklistItem, false);
            if (_checklistItem.ParentId === _checklistItem.Id) {
                this.getChecklists();
            }
        }
    }
    addChecklistItem($event, checklistItem) {
        if (checklistItem !== "" && ($event.which === 13 || $event.which === 1)) {
            var _checklistItem = {
                Id: 0,
                Description: checklistItem,
                IsCompleted: false,
                ParentId: this.selectedChecklist.Id
            };
            this._checklistServise.createChecklist(_checklistItem)
                .map(res => res.json())
                .subscribe(data => { this.setEditState(checklistItem, false); });
            this.showChecklist(this.selectedChecklist.Id);
            this.newChecklistItem = "";
        }
    }
    deleteChecklist(checklist: Checklist) {
        this._checklistServise.deleteChecklist(checklist.Id).subscribe();
        this.checklists.splice(this.checklists.indexOf(checklist), 1);
        if (checklist.Id === this.selectedChecklist.Id)
            this.deselect();
    }
    deleteChecklistItem(checklist: Checklist) {
        this._checklistServise.deleteChecklist(checklist.Id).subscribe();
        this.selectedChecklist.Items = this.selectedChecklist.Items.filter(p => p.Id !== checklist.Id);
    }
}