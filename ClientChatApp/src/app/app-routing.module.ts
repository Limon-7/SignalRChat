import { PreventLoginAccessGuard } from './core/guard/prevent-login-access.guard';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from './core/guard/auth.guard';


const chatModule = () => import("@app/chat/chat.module").then(x => x.ChatModule);
const authModule = () => import("@app/authentication/authentication.module").then(x => x.AuthenticationModule);
const routes: Routes = [
  { path: "chat", loadChildren: chatModule, canActivate: [AuthGuard] },
  { path: "auth", loadChildren: authModule, canActivate: [PreventLoginAccessGuard] },
  { path: "**", redirectTo: "/auth", pathMatch: "full" }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
