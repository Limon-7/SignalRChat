import { Message } from '@angular/compiler/src/i18n/i18n_ast';
import { Component, OnInit } from '@angular/core';
import { User } from '@app/_models/user';
import { AuthService, MessageService, UserService } from '@app/_services';



@Component({
  selector: 'app-message',
  templateUrl: './message.component.html',
  styleUrls: ['./message.component.css']
})
export class MessageComponent implements OnInit {
  user: User
  messages: Message
  constructor(private messageService: MessageService, private userService: UserService) { }

  ngOnInit(): void {
  }

}
