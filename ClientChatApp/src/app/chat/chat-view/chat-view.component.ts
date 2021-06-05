import { AfterViewChecked, Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { ChatDataService } from '@app/core/data-service/chat-data.service';
import { Message } from '@app/core/model/message';
import { User } from '@app/core/model/user';
import { AuthService } from '@app/core/service/auth.service';
import { SignalRService } from '@app/core/service/signalR.service';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-chat-view',
  templateUrl: './chat-view.component.html',
  styleUrls: ['./chat-view.component.scss']
})
export class ChatViewComponent implements OnInit, AfterViewChecked {

  messageForm: FormGroup
  messages$: Observable<Message[]>;
  newMessage: any = {};
  otherUser: User
  constructor(private route: ActivatedRoute, private messageService: ChatDataService, private signalRService: SignalRService, private fb: FormBuilder, public authService: AuthService) { }
  id: number;
  recipientId: number;
  userName: string;
  @ViewChild('textContainer') private textContainer: ElementRef;
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

  ngAfterViewChecked(): void {
    if (this.textContainer) {
      this.textContainer.nativeElement.scrollTop = this.textContainer.nativeElement.scrollHeight;
    }
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
  sendMessage() {
    this.newMessage.recipientId = this.recipientId;
    this.newMessage.content = this.messageForm.get('content').value
    let senderId = this.id;
    //this.newMessage = Object.assign({}, this.messageForm.value);
    this.messageService.sendMessage(senderId, this.newMessage).subscribe(res => {
      this.newMessage.senderId = senderId;
      //this.signalRService.getSignleUserMessage(this.newMessage);
      // send message to caller
      this.signalRService.sendMessageToCaller(this.newMessage)
      console.log("message send succesfully", this.newMessage);
      this.messageForm.reset();
    })

  }
  deleteMessage(messageId: number) {
    this.messageService.deleteMessageOneSide(messageId, this.id,).subscribe(() => {
    },
      err => { console.log(err) })
  }
  deletMessageBoth(messageId: number) {
    this.messageService.deleteMessageBothSide(messageId, this.id,).subscribe(() => {
    },
      err => { console.log(err) })
  }
  deleteConversation() {
    this.messageService.deleteConversation(this.id, this.otherUser.id).subscribe(() => {
    },
      err => { console.log(err) })
  }
  //get seigle user
  getSingleUser(id: number) {
    this.authService.getUserById(id).subscribe((res) => {
      this.otherUser = res;
      console.log("User-message", this.otherUser)
    }, err => { console.log(err) })
  }

}
