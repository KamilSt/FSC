import { NgModule, } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppComponent } from './app.component';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { RouterModule, Routes } from '@angular/router';

import { PlCurrencyPipe } from "./pipes/PlCurrency.pipe";
import { PaginatePipe } from "./pipes/paginate/paginate.pipe";
import { PaginateComponent } from "./pipes/paginate/paginate.component";

import { NavbarComponent } from './navbar.component';
import { ChecklistsComponent } from './checklist/checklist.component';
import { HomepageComponent } from "./homepage.component";
import { SalaryCalculatorComponent } from "./salaryCalculator/salaryCalculator.component";
import { OrdersComponent } from "./orders/orders.component";
import { NewOrderComponent } from "./orders/newOrder.component";
import { CustomersComponent } from "./customer/customers.component";
import { CustomerEditComponent } from "./customer/customerEdit.component";
import { CustomerSelector } from "./customer/customerSelector.component";
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
    imports: [BrowserModule, FormsModule, HttpModule, RouterModule.forRoot(appRoutes)],
    declarations: [AppComponent,
        ChecklistsComponent,
        NavbarComponent,
        HomepageComponent,
        SalaryCalculatorComponent,
        OrdersComponent,
        NewOrderComponent,
        CustomersComponent,
        CustomerEditComponent,
        FinancialDocumentComponent,
        CustomerSelector,
        PlCurrencyPipe],
    bootstrap: [AppComponent, NavbarComponent]
})
export class AppModule { }
