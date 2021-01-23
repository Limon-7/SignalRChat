import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { AuthService } from '../service/auth.service';

@Injectable({
  providedIn: 'root'
})
export class PreventLoginAccessGuard implements CanActivate {
  constructor(private authService: AuthService, private router: Router) { }
  canActivate(): boolean {
    if (this.authService.loggedIn()) {
      return true;
    }

    this.router.navigate(['/chat']);
    return false;
  }

}
