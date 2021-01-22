import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';


import { LoginComponent } from './shared/login/login.component';
import { RegisterComponent } from './shared/register/register.component';
const chatModule = () => import("./chat/chat.module").then(x => x.ChatModule);
const routes: Routes = [

  {
    path: "login", component: LoginComponent,

  },
  {
    path: "register", component: RegisterComponent,

  },
  { path: "chat", loadChildren: chatModule },
  // { path: "chat", component: ChatLayoutComponent },
  // {
  //   path: "**", redirectTo: "", pathMatch: "full"
  // }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
