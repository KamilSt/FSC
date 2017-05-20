import { Component } from "@angular/core";
import { NavbarComponent } from './navbar.component';

@Component({
    selector: "my-app",
    template: "  <navbar></navbar><router-outlet></router-outlet>"
    //directives : []
})
export class AppComponent {
}