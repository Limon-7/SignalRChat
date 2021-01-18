import { Injectable } from '@angular/core';
import { User } from '@app/_models';
import { HubConnection, HubConnectionBuilder, LogLevel } from '@aspnet/signalr';
import { BehaviorSubject, Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SignalRService {
  messageReceived$ = new Subject();
  userList$ = new Subject<User>();
  connectionEstablished$ = new BehaviorSubject<boolean>(false);
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
        console.log('Hub connection started!');
        this.connectionEstablished$.next(true);
      }
    ).catch(err => console.log(err));
  }
  private registerOnServerEvents(): void {
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
}
