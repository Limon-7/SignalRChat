import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder, LogLevel } from '@aspnet/signalr';
import { BehaviorSubject, Subject } from 'rxjs';
import { User } from '../model/user';

@Injectable({
  providedIn: 'root'
})
export class SignalRService {
  messageReceived$ = new Subject();
  userList$ = new Subject<User>();
  connectionId: any
  connectionEstablished$ = new BehaviorSubject<boolean>(false);
  // private loginUser = JSON.parse(localStorage.getItem['user']);
  // private userId = this.loginUser['id'];
  private hubConnection: HubConnection;
  constructor() {
    this.createConnection();
    this.hubConnectionStart();
    this.registerOnServerEvents();
  }

  private createConnection() {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(' http://localhost:5000/notify')
      .configureLogging(LogLevel.Information)
      .build();
  }
  private hubConnectionStart() {
    this.hubConnection.start().then(
      () => {
        // this.getConnectionId();

        console.log('Hub connection started!');
        this.connectionEstablished$.next(true);
      }
    ).catch(err => console.log(err));
  }
  public getConnectionId = () => {
    this.hubConnection.invoke('getconnectionid').then(
      (data) => {
        console.log(data);
        this.connectionId = data;
      }
    );
  }
  private registerOnServerEvents(): void {
    // this.hubConnection.invoke('receivedMessage').then((data: any) => {
    //   console.log("message:added:", data)
    //   this.messageReceived$.next(data);
    // });
    this.hubConnection.on('receivedMessage', (data: any) => {
      console.log("message:added:", data)
      this.messageReceived$.next(data);
    });
    this.hubConnection.on('messageDelete', (data: any) => {
      console.log("message:added:", data)
      this.messageReceived$.next("Message deleted");
    });
    this.hubConnection.on('refreshUsers', (data: any) => {
      this.userList$.next(data);
    });
  }
  public getSignleUserMessage(data: any) {
    this.hubConnection.send('broadCast', data);
  }
}
