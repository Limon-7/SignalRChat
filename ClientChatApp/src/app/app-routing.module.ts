import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './home/home.component';

const chatModule = () => import("@app/chat/chat.module").then(x => x.ChatModule);
const authModule = () => import("@app/authentication/authentication.module").then(x => x.AuthenticationModule);
const routes: Routes = [
  { path: "chat", loadChildren: chatModule },
  { path: "auth", loadChildren: authModule },
  { path: "home", component: HomeComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
