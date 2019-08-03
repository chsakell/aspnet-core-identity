import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpModule, Http } from '@angular/http';
import { RouterModule } from '@angular/router';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {ToastModule} from 'ng2-toastr/ng2-toastr';

import { AppComponent } from './components/app/app.component';
import { NavMenuComponent } from './components/navmenu/navmenu.component';
import { HomeComponent } from './components/home/home.component';
import { RegisterComponent } from './components/register/register.component';
import { LoginComponent } from './components/login/login.component';
import { ClaimsComponent } from './components/claims/claims.component';
import { StreamingComponent } from './components/streaming/streaming.component';
import { HttpInterceptor } from './core/http.interceptor';
import { StateService } from './core/state.service';
import { AccessForbiddenComponent } from './components/claims/access-forbidden/access-forbidden.component';
import { ManageComponent } from './components/manage/manage.component';
import { StreamingRegisterComponent } from './components/streaming/register/register.component';
import { AddVideoComponent } from './components/streaming/add/add-video.component';
import { SocialApiShareComponent } from './components/socialapi/share.component';
import { OpenIdConnectService } from './core/openid-connect.service';
import { PasswordComponent } from './components/password/password.component';
import { AccountComponent } from './components/account/account.component';

@NgModule({
    declarations: [
        AppComponent,
        ClaimsComponent,
        NavMenuComponent,
        LoginComponent,
        RegisterComponent,
        HomeComponent,
        StreamingComponent,
        StreamingRegisterComponent,
        AddVideoComponent,
        AccessForbiddenComponent,
        ManageComponent,
        SocialApiShareComponent,
        PasswordComponent,
        AccountComponent
    ],
    imports: [
        CommonModule,
        HttpModule,
        FormsModule,
        BrowserAnimationsModule, 
        ToastModule.forRoot(),
        RouterModule.forRoot([
            { path: '', redirectTo: 'home', pathMatch: 'full' },
            { path: 'home', component: HomeComponent },
            { path: 'login', component: LoginComponent },
            { path: 'register', component: RegisterComponent },
            { path: 'claims', component: ClaimsComponent },
            { path: 'videos/:id', component: StreamingComponent },
            { path: 'videos', component: StreamingComponent },
            { path: 'streaming/register', component: StreamingRegisterComponent },
            { path: 'streaming/videos/add', component: AddVideoComponent },
            { path: 'access-forbidden', component: AccessForbiddenComponent },
            { path: 'manage/users', component: ManageComponent },
            { path: 'password', component: PasswordComponent },
            { path: 'share', component: SocialApiShareComponent },
            { path: 'manage/account', component: AccountComponent },
            { path: '**', redirectTo: 'home' }
        ])
    ],
    providers: [
        { provide: Http, useClass: HttpInterceptor },
        StateService,
        OpenIdConnectService
    ]
})
export class AppModuleShared {
}
