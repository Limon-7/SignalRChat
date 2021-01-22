import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-chat-user-list',
  templateUrl: './chat-user-list.component.html',
  styleUrls: ['./chat-user-list.component.css']
})
export class ChatUserListComponent implements OnInit {
  chats: any[];
  chatSearch: any;
  contacts: [
    { id: 1, name: "limon" }
  ];
  searchText: string;
  user: any;
  constructor() { }

  ngOnInit(): void {
  }
  getChat(contact): void {

  }
}
