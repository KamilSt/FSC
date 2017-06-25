import { NgModule, } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppComponent } from './app.component';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { RouterModule, Routes } from '@angular/router';

import { NavbarComponent } from './navbar.component';
import { ChecklistsComponent } from './checklist/checklist.component';
import { HomepageComponent } from "./homepage.component";
import { SalaryCalculatorComponent } from "./salaryCalculator/salaryCalculator.component";
import { OrdersComponent } from "./orders/orders.component";
import {NewOrderComponent } from "./orders/newOrder.component";

const appRoutes: Routes = [
    { path: '', component: HomepageComponent },
    { path: 'home', component: HomepageComponent },
    { path: 'orders', component: OrdersComponent },
    { path: 'newOrder', component: NewOrderComponent },
    { path: 'newOrder/:id', component: NewOrderComponent },
    { path: 'salaryCalc', component: SalaryCalculatorComponent },
    { path: 'checkLists', component: ChecklistsComponent },
    { path: '**', component: HomepageComponent }
];

@NgModule({
    imports: [BrowserModule, FormsModule, HttpModule, RouterModule.forRoot(appRoutes)],
    declarations: [AppComponent, ChecklistsComponent, NavbarComponent, HomepageComponent, SalaryCalculatorComponent, OrdersComponent, NewOrderComponent],
    bootstrap: [AppComponent,  NavbarComponent]
})
export class AppModule { }
