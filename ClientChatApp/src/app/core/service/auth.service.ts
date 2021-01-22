import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Router } from "@angular/router";
import { User } from "../model/user";
import { JwtHelperService } from "@auth0/angular-jwt";
import { environment } from "@environments/environment";
import { throwError } from "rxjs";
import { catchError } from 'rxjs/operators';
import { map } from "rxjs/operators";

const baseUrl = environment.apiUrl + "/auth"
@Injectable({
  providedIn: 'root'
})
export class AuthService {

  jwtHelper = new JwtHelperService();
  decodedToken: any;
  currentUser: User;
  constructor(private http: HttpClient, private route: Router) { }
  login(email: any) {
    return this.http.post(baseUrl + "/login", { email }).pipe(
      catchError(this.handleError),
      map((response: any) => {

        const user = response;
        if (user) {
          localStorage.setItem('token', user.token);
          localStorage.setItem('user', JSON.stringify(user.user));
          this.decodedToken = this.jwtHelper.decodeToken(user.token);
          this.currentUser = user.user;
          return response;
        }
        else {
          this.route.navigate(['/login']);
        }
      })
    );
  }
  register(user: User): any {
    return this.http.post(baseUrl + '/register', user).pipe(
      catchError(this.handleError));
  }
  logout(): void {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    this.decodedToken = null;
    this.currentUser = null;
    this.route.navigate(['/login']);
  }
  loggedIn(): any {
    const token = localStorage.getItem('token');
    return !this.jwtHelper.isTokenExpired(token);
  }

  getUsers(id: number): any {
    return this.http.get(`${baseUrl}/all/${id}`).pipe(catchError(this.handleError));
  }
  getUserById(id: number): any {
    return this.http.get(`${baseUrl}/${id}`).pipe(catchError(this.handleError));
  }
  private handleError(error: Response) {
    return throwError(error || 'Server error');
  }
}
