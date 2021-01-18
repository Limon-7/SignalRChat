
import { HttpClient } from '@angular/common/http';
import { EventEmitter, Injectable } from '@angular/core';
import { User } from '@app/_models';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';
import { environment } from '@environments/environment';
import { throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

const baseUrl = environment.apiUrl + "users"
@Injectable({
  providedIn: 'root'
})
export class UserService {
  private hubConnection: HubConnection;
  users: User[]
  onMessageReceived = new EventEmitter<any>();
  constructor(private http: HttpClient) { }
  /* start connection */
  public startConnection = () => {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(`http://localhost:5000/notifyUser`).build();

    this.hubConnection
      .start()
      .then(() => console.log("hub connected"))
      .catch(err => console.log('Error while starting connection: ' + err));


  }
  public notifyUserList = () => {
    this.hubConnection.on("getUser", (data) => {
      this.users = data;
      console.log("signalR Data:", data)
    })
  }
  getUsers(): any {
    return this.http.get(baseUrl).pipe(catchError(this.handleError));
  }
  getUserById(id: number): any {
    return this.http.get(`${baseUrl}/${id}`).pipe(catchError(this.handleError));
  }
  private handleError(error: Response) {
    return throwError(error || 'Server error');
  }
}
