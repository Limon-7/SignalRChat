import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MessageComponent } from './message/message.component';
import { MessageRoutingModule } from './message-routing-module';
import { UserListComponent } from './user-list/user-list.component';
import { MessageDetailsComponent } from './message-details/message-details.component';
import { UserListItemComponent } from './user-list-item/user-list-item.component';
import { ReactiveFormsModule } from '@angular/forms';




@NgModule({
  declarations: [MessageComponent, UserListComponent, MessageDetailsComponent, UserListItemComponent],
  imports: [
    CommonModule,
    MessageRoutingModule,
    ReactiveFormsModule
  ]
})
export class MessageModule { }
