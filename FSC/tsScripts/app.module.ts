import { NgModule, } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppComponent } from './app.component';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { RouterModule, Routes } from '@angular/router';

import { NavbarComponent } from './navbar.component';
import { ChecklistsComponent } from './checklist/checklist.component';
import { HomepageComponent } from "./homepage.component";
import { SalaryCalculatorComponent } from "./salaryCalculator.component";

const appRoutes: Routes = [
    { path: '', component: HomepageComponent },
    { path: 'home', component: HomepageComponent },
    { path: 'salaryCalc', component: SalaryCalculatorComponent },
    { path: 'checkLists', component: ChecklistsComponent },
    { path: '**', component: HomepageComponent }
];

@NgModule({
    imports: [BrowserModule, FormsModule, HttpModule, RouterModule.forRoot(appRoutes)],
    declarations: [AppComponent, ChecklistsComponent, NavbarComponent, HomepageComponent, SalaryCalculatorComponent],
    bootstrap: [AppComponent, ChecklistsComponent, NavbarComponent]
})
export class AppModule { }
