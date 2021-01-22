import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ChatUserListComponent } from './chat-user-list/chat-user-list.component';
import { ChatViewComponent } from './chat-view/chat-view.component';
import { ChatComponent } from './chat/chat.component';

const routes: Routes = [
  {
    path: "", component: ChatComponent, children: [
      { path: 'user/:id', component: ChatViewComponent },
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ChatRoutingModule { }
