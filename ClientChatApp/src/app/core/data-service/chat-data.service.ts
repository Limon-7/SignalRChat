import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@environments/environment';
import { throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Message } from '../model/message';

const baseUrl = environment.apiUrl + "user";
@Injectable({
  providedIn: 'root'
})
export class ChatDataService {

  constructor(private http: HttpClient) { }

  getMessageThread(id: number, recipientId: number) {
    return this.http.get<Message[]>(`${baseUrl}/${id}/messages/thread/${recipientId}`).pipe(catchError(this.handleError));
  }
  deleteMessage(id: number, userId: number) {
    return this.http.delete(`${baseUrl}/${userId}/messages/${id}`, {}).pipe(catchError(this.handleError));
  }
  sendMessage(id: number, message: Message) {
    return this.http.post(`${baseUrl}/${id}/messages`, message).pipe(catchError(this.handleError));
  }
  private handleError(error: Response) {
    return throwError(error || 'Server error');
  }
}
