import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { MessageComponent } from './message/message.component';
import { MessageDetailsComponent } from './message-details/message-details.component';


const routes: Routes = [
    {
        path: "", component: MessageComponent, children: [
            { path: 'user/:id', component: MessageDetailsComponent },
        ]
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class MessageRoutingModule { }
