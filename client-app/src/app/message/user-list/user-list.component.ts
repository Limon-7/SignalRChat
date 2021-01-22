import { Component, NgZone, OnInit } from '@angular/core';
import { User } from '@app/_models';
import { AuthService, SignalRService } from '@app/_services';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.css']
})
export class UserListComponent implements OnInit {
  users$: Observable<User[]>;
  constructor(private authService: AuthService, private signalRService: SignalRService) { }

  ngOnInit(): void {
    this.signalRService.userList$.subscribe(() => this.getUsersData());

    this.getUsersData();
  }
  private getUsersData() {
    let user = JSON.parse(localStorage.getItem('user'));
    let id = user['id'];
    // console.log("user-id", id)
    this.users$ = this.authService.getUsers(id);
    //  let data = this.users$.subscribe(res => {console.log(res)});
    // console.log(data)
  }

}
