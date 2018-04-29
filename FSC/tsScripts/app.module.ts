import { NgModule, } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppComponent } from './app.component';
import { FormsModule, ReactiveFormsModule,  } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { RouterModule, Routes } from '@angular/router';

import { PlCurrencyPipe } from "./pipes/PlCurrency.pipe";
import { PaginatePipe } from "./pipes/paginate/paginate.pipe";

import { NavbarComponent } from './navbar.component';
import { ChecklistsComponent } from './checklist/checklist.component';
import { HomepageComponent } from "./homepage.component";
import { SalaryCalculatorComponent } from "./salaryCalculator/salaryCalculator.component";
import { OrdersComponent } from "./orders/orders.component";
import { NewOrderComponent } from "./orders/newOrder.component";
import { FilterComponent } from "./components/filters/filter.component";
import { DynamicFormComponent } from './components/filters/dynamicForm/dynamic-form.component';
import { DynamicFormQuestionComponent } from './components/filters/dynamicForm/dynamic-form-question.component';
import { CustomersComponent } from "./customer/customers.component";
import { CustomerEditComponent } from "./customer/customerEdit.component";
import { CustomerSelector } from "./customer/customerSelector.component";
import { PaginateComponent } from "./pipes/paginate/paginate.component";
import { FinancialDocumentComponent } from "./financialDocuments/financialDocuments.component";

const appRoutes: Routes = [
    { path: '', component: HomepageComponent },
    { path: 'home', component: HomepageComponent },
    { path: 'orders', component: OrdersComponent },
    { path: 'newOrder', component: NewOrderComponent },
    { path: 'newOrder/:id', component: NewOrderComponent },
    { path: 'salaryCalc', component: SalaryCalculatorComponent },
    { path: 'checkLists', component: ChecklistsComponent },
    { path: 'customers', component: CustomersComponent },
    { path: 'financialDocuments', component: FinancialDocumentComponent },
    { path: '**', component: HomepageComponent }
];

@NgModule({
    imports: [BrowserModule, FormsModule, HttpModule, ReactiveFormsModule, RouterModule.forRoot(appRoutes)],
    declarations: [AppComponent,
        ChecklistsComponent,
        NavbarComponent,
        HomepageComponent,
        SalaryCalculatorComponent,
        OrdersComponent,
        NewOrderComponent,
        FilterComponent, DynamicFormComponent, DynamicFormQuestionComponent,
        CustomersComponent, PaginateComponent,
        CustomerEditComponent,
        FinancialDocumentComponent,
        CustomerSelector,
        PlCurrencyPipe, PaginatePipe],
    bootstrap: [AppComponent, NavbarComponent]
})
export class AppModule { }
