import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@environments/environment';
import { throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Message } from '../model/message';

const baseUrl = environment.apiUrl + "/messages";
@Injectable({
  providedIn: 'root'
})
export class ChatDataService {

  constructor(private http: HttpClient) { }

  getMessageThread(id: number, recipientId: number) {
    return this.http.get<Message[]>(`${baseUrl}/${id}/thread/${recipientId}`).pipe(catchError(this.handleError));
  }
  deleteMessageOneSide(messageId: number, userId: number) {
    return this.http.delete(`${baseUrl}/${userId}/delete/${messageId}`, {}).pipe(catchError(this.handleError));
  }
  deleteMessageBothSide(messageId: number, userId: number) {
    return this.http.delete(`${baseUrl}/${userId}/delete/both/${messageId}`, {}).pipe(catchError(this.handleError));
  }
  deleteConversation(userId: number, recipientId: number,) {
    return this.http.delete(`${baseUrl}/${userId}/delete/all/${recipientId}`, {}).pipe(catchError(this.handleError));
  }
  sendMessage(id: number, message: Message) {
    return this.http.post(`${baseUrl}/${id}`, message).pipe(catchError(this.handleError));
  }
  private handleError(error: Response) {
    return throwError(error || 'Server error');
  }
}
