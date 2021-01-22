import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ChatRoutingModule } from './chat-routing.module';
import { ChatComponent } from './chat/chat.component';
import { SharedModule } from '@app/shared/shared.module';
import { ChatSidenavComponent } from './chat-sidenav/chat-sidenav.component';
import { ChatUserListComponent } from './chat-user-list/chat-user-list.component';
import { ChatViewComponent } from './chat-view/chat-view.component';
import { ChatStartComponent } from './chat-start/chat-start.component';
import { FormsModule } from '@angular/forms';


@NgModule({
  declarations: [ChatComponent, ChatSidenavComponent, ChatUserListComponent, ChatViewComponent, ChatStartComponent],
  imports: [
    CommonModule,
    ChatRoutingModule,
    SharedModule,
    FormsModule

  ]
})
export class ChatModule { }
