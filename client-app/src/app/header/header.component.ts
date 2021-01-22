import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {
  user = JSON.parse(localStorage.getItem('user'))
  constructor(public authService: AuthService) { }

  ngOnInit(): void {
    // console.log("i am here")
  }

}
