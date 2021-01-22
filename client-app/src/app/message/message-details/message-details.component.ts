
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Message, User } from '@app/_models';
import { AuthService, MessageService, SignalRService } from '@app/_services';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-message-details',
  templateUrl: './message-details.component.html',
  styleUrls: ['./message-details.component.css']
})
export class MessageDetailsComponent implements OnInit {

  messageForm: FormGroup
  messages$: Observable<Message[]>;
  newMessage: any = {};
  otherUser: User
  constructor(private route: ActivatedRoute, private messageService: MessageService, private signalRService: SignalRService, private fb: FormBuilder, private authService: AuthService) { }
  id: number;
  recipientId: number;
  userName: string;

  ngOnInit(): void {
    this.createMessageForm();
    let user = JSON.parse(localStorage.getItem("user"));
    this.userName = user['firstName']
    this.id = user['id']

    this.route.paramMap.subscribe(params => {
      this.recipientId = parseInt(params.get("id"));
      this.loadMessagesData(this.id, this.recipientId)
      this.getSingleUser(this.recipientId)
      console.log(`Id:${this.id} with RecippientId:${this.recipientId}`)
    });
    this.signalRService.messageReceived$.subscribe(() => this.loadMessagesData(this.id, this.recipientId));
    this.loadMessagesData(this.id, this.recipientId)
  }
  private loadMessagesData(userId: number, recipientid: number) {
    this.messages$ = this.messageService.getMessageThread(userId, recipientid);
    this.messages$.subscribe(data => { console.log("message", data) })
  }
  createMessageForm() {
    this.messageForm = this.fb.group({
      content: ["", [Validators.required]]
    })
  }
  get f() { return this.messageForm.controls; }
  messageSend() {
    this.newMessage.recipientId = this.recipientId;
    this.newMessage.content = this.messageForm.get('content').value
    // this.newMessage = Object.assign({}, this.messageForm.value);
    let senderId = this.id;
    this.messageService.sendMessage(senderId, this.newMessage).subscribe(res => {
      this.newMessage.senderId = senderId;
      this.signalRService.getSignleUserMessage(this.newMessage);
      console.log("message send succesfully", this.newMessage);
      this.messageForm.reset();
    })

  }
  deleteMessage(messageId: number) {
    this.messageService.deleteMessage(messageId, this.id,).subscribe(() => {
    },
      err => { console.log(err) })
  }
  //get seigle user
  getSingleUser(id: number) {
    this.authService.getUserById(id).subscribe((res) => {
      this.otherUser = res;
      // console.log("User", this.otherUser)
    }, err => { console.log(err) })
  }
}
