import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { AuthGuard } from './_guard/auth.guard';
import { PreventLoginAccess } from './_guard/pevent-login-access';

const messageModule = () => import("./message/message.module").then(x => x.MessageModule);
const routes: Routes = [
  { path: "", component: HomeComponent, canActivate: [PreventLoginAccess] },
  { path: "login", component: LoginComponent, canActivate: [PreventLoginAccess] },
  { path: "register", component: RegisterComponent, canActivate: [PreventLoginAccess] },
  { path: "message", loadChildren: messageModule, canActivate: [AuthGuard] },
  // { path: "message", loadChildren: messageModule },
  // { path: '**', redirectTo: '/404' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
