
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { HomeComponent } from './home/home.component';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { HeaderComponent } from './header/header.component';
import { PreventLoginAccess } from './_guard/pevent-login-access';
import { AuthGuard } from './_guard/auth.guard';

import { JwtInterceptor } from './_helper/jwt.interceptor';
import { MessageService, AuthService, SignalRService } from '@app/_services';


@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    RegisterComponent,
    HomeComponent,
    HeaderComponent,


  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    ReactiveFormsModule,
    HttpClientModule,
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
    PreventLoginAccess,
    AuthGuard,
    AuthService,
    MessageService,
    SignalRService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
