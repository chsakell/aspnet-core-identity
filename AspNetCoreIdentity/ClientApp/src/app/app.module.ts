import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './navmenu/navmenu.component';
import { HomeComponent } from './home/home.component';
import { ClaimsComponent } from './claims/claims.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { ManageAccountComponent } from './manage-account/manage-account.component';
import { AccessForbiddenComponent } from './claims/access-forbidden/access-forbidden.component';
import { AdminComponent } from './admin/admin.component';
import { AccountProfileComponent } from './manage-account/account-profile.component';
import { SetupAuthenticatorComponent } from './manage-account/setup-authenticator.component';
import { ResetAuthenticatorComponent } from './manage-account/reset-authenticator.component';
import { StreamingComponent } from './streaming/streaming.component';
import { StreamingRegisterComponent } from './streaming/register/register.component';
import { AddVideoComponent } from './streaming/add/add-video.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';

import { httpInterceptorProviders } from './core/http-interceptors/index';
import { StateService } from './core/state.service';
import { OpenIdConnectService } from './core/openid-connect.service';
import { PasswordComponent } from './password/password.component';
import { SocialApiShareComponent } from './socialapi/share.component';


@NgModule({
  declarations: [
    AppComponent,
    ClaimsComponent,
    LoginComponent,
    RegisterComponent,
    AccessForbiddenComponent,
    AdminComponent,
    ManageAccountComponent,
    AccountProfileComponent,
    SetupAuthenticatorComponent,
    ResetAuthenticatorComponent,
    NavMenuComponent,
    HomeComponent,
    StreamingComponent,
    StreamingRegisterComponent,
    AddVideoComponent,
    SocialApiShareComponent,
    PasswordComponent,
    CounterComponent,
    FetchDataComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', redirectTo: 'home', pathMatch: 'full' },
      { path: 'home', component: HomeComponent },
      { path: 'login', component: LoginComponent },
      { path: 'register', component: RegisterComponent },
      { path: 'access-forbidden', component: AccessForbiddenComponent },
      { path: 'manage/users', component: AdminComponent },
      { path: 'password', component: PasswordComponent },
      { path: 'share', component: SocialApiShareComponent },
      { path: 'claims', component: ClaimsComponent },
      { path: 'videos/:id', component: StreamingComponent },
      { path: 'videos', component: StreamingComponent },
      { path: 'streaming/register', component: StreamingRegisterComponent },
      { path: 'streaming/videos/add', component: AddVideoComponent },
      { path: 'manage/account', component: ManageAccountComponent },
      { path: 'counter', component: CounterComponent },
      { path: 'fetch-data', component: FetchDataComponent },
      { path: '**', redirectTo: 'home' }
    ])
  ],
  providers: [
    httpInterceptorProviders,
    StateService,
    OpenIdConnectService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
