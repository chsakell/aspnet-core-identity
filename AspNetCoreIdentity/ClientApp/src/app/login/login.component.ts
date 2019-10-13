import { Component, Inject, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { StateService } from '../core/state.service';
import { LoginVM, ResultVM, StatusEnum } from '../core/domain';

@Component({
    selector: 'login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
    public user: LoginVM = { username: '', password: '' }
    public errors: string = '';
    public socialProviders: string[] = [];
    public requires2FA: boolean = false;
    public twoFaCode: string = '';
    public useRecoveryCode: boolean = false;
    public rememberMachine: boolean = false;

    constructor(public http: HttpClient,
                @Inject('BASE_URL') public baseUrl: string,
                public router: Router, public stateService: StateService) {
    }

    ngOnInit() {
        this.http.get<string[]>(this.baseUrl + 'socialaccount/providers')
            .subscribe(result => {
                this.socialProviders = result;
                console.log(this.socialProviders);
            });
    }

    onKeydown(event: any) {
        if (event.key === "Enter") {
            this.login();
        }
    }

    twoFaPlaceholder() {
        return this.useRecoveryCode ? 'Enter recovery code' : '6-digit code';
    }

    login() {
        this.errors = '';
        console.log(this.user);

        if (this.requires2FA) {

            let data = {};
            let uri = '';

            if (this.useRecoveryCode) {
                data = {
                    recoveryCode: this.twoFaCode
                };
                uri = 'api/twoFactorAuthentication/loginWithRecovery';
            } else {
                data = {
                    twoFactorCode: this.twoFaCode,
                    rememberMachine: this.rememberMachine
                };
                uri = 'api/twoFactorAuthentication/login';
            }

            this.http.post<ResultVM>(this.baseUrl + uri, data).subscribe(result => {
                    let loginResult = result;
                    if (loginResult.status === StatusEnum.Success) {
                        this.stateService.setAuthentication({
                            isAuthenticated: true,
                            username: this.user.username,
                            authenticationMethod: ''
                        });
                        this.router.navigate(['/home']);
                    } else if (loginResult.status === StatusEnum.Error) {
                        this.errors = loginResult.data.toString();
                    }

                },
                error => console.error(error));
        } else {
            this.http.post<ResultVM>(this.baseUrl + 'api/account/login', this.user).subscribe(result => {
                    let loginResult = result;
                    if (loginResult.status === StatusEnum.Success) {
                        if (loginResult.data.requires2FA) {
                            this.requires2FA = true;
                            this.stateService.displayNotification({ message: loginResult.message, type: "success" });
                            return;
                        }
                        this.stateService.setAuthentication({
                            isAuthenticated: true,
                            username: this.user.username,
                            authenticationMethod: ''
                        });
                        this.router.navigate(['/home']);
                    } else if (loginResult.status === StatusEnum.Error) {
                        this.errors = loginResult.data.toString();
                    }

                },
                error => console.error(error));
        }
    }
}



