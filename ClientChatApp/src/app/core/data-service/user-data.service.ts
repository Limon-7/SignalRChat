import { HttpClient } from '@angular/common/http';
import { EventEmitter, Injectable } from '@angular/core';
import { environment } from '@environments/environment';
import { throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

const baseUrl = environment.apiUrl + "/auth"
@Injectable({
  providedIn: 'root'
})
export class UserDataService {

  constructor(private http: HttpClient) {
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
