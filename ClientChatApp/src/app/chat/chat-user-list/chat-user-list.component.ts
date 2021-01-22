import { Component, OnInit } from '@angular/core';
import { UserDataService } from '@app/core/data-service/user-data.service';
import { User } from '@app/core/model/user';
import { SignalRService } from '@app/core/service/signalR.service';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-chat-user-list',
  templateUrl: './chat-user-list.component.html',
  styleUrls: ['./chat-user-list.component.scss']
})
export class ChatUserListComponent implements OnInit {
  users$: Observable<User[]>;
  user = JSON.parse(localStorage.getItem("user"));
  constructor(private userService: UserDataService, private signlRService: SignalRService) { }

  ngOnInit(): void {
    this.signlRService.userList$.subscribe(() => this.getUsersData());

    this.getUsersData();
  }
  private getUsersData() {
    let user = JSON.parse(localStorage.getItem('user'));
    let id = user['id'];
    // console.log("user-id", id)
    this.users$ = this.userService.getUsers(id);
    let data = this.users$.subscribe(res => { console.log(res) });
    // console.log(data)
  }

}
