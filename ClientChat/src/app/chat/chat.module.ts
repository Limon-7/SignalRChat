import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ChatLayoutComponent } from './chat-layout/chat-layout.component';
import { ChatUserComponent } from './chat-user/chat-user.component';
import { ChatRoutingModule } from './chat-routing.module';
import { ChatDetailsComponent } from './chat-details/chat-details.component';
import { ChatLeftSidenavComponent } from './chat-left-sidenav/chat-left-sidenav.component';
import { SharedModule } from '../shared/shared.module';
import { ChatUserListComponent } from './chat-user-list/chat-user-list.component';



@NgModule({
  declarations: [ChatLayoutComponent, ChatUserComponent, ChatDetailsComponent, ChatLeftSidenavComponent, ChatUserListComponent],
  imports: [
    CommonModule,
    ChatRoutingModule,
    SharedModule
  ],
  exports: []
})
export class ChatModule { }
