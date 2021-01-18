import { Injectable } from "@angular/core";
import { CanActivate, Router } from "@angular/router";
import { AuthService } from "../_services/auth.service";

@Injectable()
export class PreventLoginAccess implements CanActivate {
    constructor(private authService: AuthService, private route: Router) { }
    canActivate() {
        if (!this.authService.loggedIn()) {
            console.log("token:", this.authService.loggedIn())
            return true
        }
        this.route.navigate["/login"]
        return false
    }
}