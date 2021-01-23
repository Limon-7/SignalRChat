import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from '../service/auth.service';
import { environment } from 'src/environments/environment';
import { tap } from 'rxjs/operators';
import { Router } from '@angular/router';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {

  constructor(private authService: AuthService, private route: Router) { }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    // set default http header
    const currentUser = localStorage.getItem("user");
    const token = localStorage.getItem("token")
    const isApiUrl = request.url.startsWith(environment.apiUrl);
    if (token && isApiUrl && currentUser) {
      request = request.clone({
        setHeaders: { Authorization: `Bearer ${token}` }
      });
    }
    return next.handle(request).pipe(
      tap(
        succ => { },
        err => {
          if (err.status == 401) {
            localStorage.removeItem('token');
            this.route.navigateByUrl('/auth/login');
          }
          // else if(err.status == 403)
          // this.route.navigateByUrl('/forbidden');
        }
      )
    );
  }
}
