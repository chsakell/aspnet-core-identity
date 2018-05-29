import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpModule, Http } from '@angular/http';
import { RouterModule } from '@angular/router';

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

@NgModule({
    declarations: [
        AppComponent,
        ClaimsComponent,
        NavMenuComponent,
        LoginComponent,
        RegisterComponent,
        HomeComponent,
        StreamingComponent,
        AccessForbiddenComponent,
        ManageComponent
    ],
    imports: [
        CommonModule,
        HttpModule,
        FormsModule,
        RouterModule.forRoot([
            { path: '', redirectTo: 'home', pathMatch: 'full' },
            { path: 'home', component: HomeComponent },
            { path: 'login', component: LoginComponent },
            { path: 'register', component: RegisterComponent },
            { path: 'claims', component: ClaimsComponent },
            { path: 'videos', component: StreamingComponent },
            { path: 'access-forbidden', component: AccessForbiddenComponent },
            { path: 'manage/users', component: ManageComponent },
            { path: '**', redirectTo: 'home' }
        ])
    ],
    providers: [
        { provide: Http, useClass: HttpInterceptor },
        StateService
    ]
})
export class AppModuleShared {
}
