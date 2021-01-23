import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { JsonHubProtocol } from '@aspnet/signalr';
import { AuthService } from '../service/auth.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(private authService: AuthService, private router: Router) { }
  canActivate(): boolean {
    if (this.authService.loggedIn()) {
      console.log("this.authService.loggedIn()", this.authService.loggedIn())
      // this.router.navigate(['/chat']);
      return true;
    }
    return false;
  }

}
