import { Component } from "@angular/core";
import { Store } from "../services/store.service";
import { Router } from "@angular/router";
import { LoginRequest } from "../shared/LoginResults";

@Component({
    "selector": "login-page",
    "templateUrl": "login.component.html"
})
export class LoginPage {
    constructor(private store: Store, private router: Router) { }

    public creds: LoginRequest = {
        username: "",
        password: ""
    }

    public errorMessage = "";

    onLogin() {
        this.store.login(this.creds).subscribe(() =>
        {
            // Success
            if (this.store.order.items.length > 0)
                this.router.navigate(["checkout"]);
            else
                this.router.navigate([""]);
        }, error => {
            // Error
            this.errorMessage = `Failed to login: ${error.message}`;
            console.log(error);
        });
    }
}