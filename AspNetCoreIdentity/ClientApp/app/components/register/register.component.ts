import { Component, Inject } from '@angular/core';
import { Http } from '@angular/http';
import { Router } from '@angular/router';
import { StateService } from '../../core/state.service';

@Component({
    selector: 'register',
    templateUrl: './register.component.html',
    styleUrls: ['./register.component.css']
})
export class RegisterComponent {
    public user: RegisterVM = {
        userName: '',
        email: '',
        password: '',
        confirmPassword: '',
        startFreeTrial: true,
        isAdmin: false
    };
    public errors: string = '';
    public simpleUser: boolean = false;
    public externalRegistration = {
        username: '',
        associate: false,
        associateExistingAccount: false,
        originalEmail: '',
        associateEmail: '',
        loginProvider: '',
        providerDisplayName: '',
        providerKey: ''
    };

    constructor(public http: Http,
        @Inject('BASE_URL') public baseUrl: string,
        public router: Router, public stateService: StateService) {
        if (this.getUrlParameter("associate")) {
            this.externalRegistration.associate = true;
            this.externalRegistration.originalEmail = this.getUrlParameter("associate");
            this.externalRegistration.loginProvider = this.getUrlParameter("loginProvider");
            this.externalRegistration.providerDisplayName = this.getUrlParameter("providerDisplayName");
            this.externalRegistration.providerKey = this.getUrlParameter("providerKey");
            this.user.email = this.externalRegistration.originalEmail;
        }
    }

    register() {
        this.errors = '';
        if (!this.externalRegistration.associate) {
            this.http.post(this.baseUrl + 'api/account/register', this.user).subscribe(result => {
                let registerResult = result.json() as ResultVM;
                if (registerResult.status === StatusEnum.Success) {
                    this.stateService.displayNotification({ message: registerResult.message, type: "success" });
                    this.router.navigate(['/login']);
                } else if (registerResult.status === StatusEnum.Error) {
                    this.errors = registerResult.data.toString();
                }

            },
                error => console.error(error));
        } else {
            this.externalRegistration.username = this.user.userName;

            this.http.post(this.baseUrl + 'api/externalaccount/associate', this.externalRegistration).subscribe(result => {
                console.log(this.externalRegistration);
                let registerResult = result.json() as ResultVM;
                if (registerResult.status === StatusEnum.Success) {
                    this.stateService.displayNotification({ message: registerResult.message, type: "success" });

                    if (registerResult.data && registerResult.data.username) {
                        this.stateService.setAuthentication(
                            {
                                isAuthenticated: true,
                                username: registerResult.data.username
                            });
                    }

                    this.router.navigate(['/']);
                } else if (registerResult.status === StatusEnum.Error) {
                    this.errors = registerResult.data.toString();
                }
            },
                error => console.error(error));
        }
    }

    makeAdmin(event: any) {
        if (event) {
            this.user.startFreeTrial = false;
            this.simpleUser = false;
        }
    }

    makeTrial(event: any) {
        if (event) {
            this.user.isAdmin = false;
            this.simpleUser = false;
        }
    }

    setAssociateEmail(event: any) {
        if (!event) {
            this.externalRegistration.associateEmail = '';
        }
    }

    isSimpleUser(event: any) {
        this.simpleUser = event;
        if (event) {
            this.user.isAdmin = false;
            this.user.startFreeTrial = false;
        }
    }

    private getUrlParameter(name: string) {
        name = name.replace(/[\[]/, '\\[').replace(/[\]]/, '\\]');
        const regex = new RegExp('[\\?&]' + name + '=([^&#]*)');
        const results = regex.exec(location.search);
        return results === null ? '' : decodeURIComponent(results[1].replace(/\+/g, ' '));
    }
}

interface RegisterVM {
    userName: string;
    email: string;
    password: string;
    confirmPassword: string;
    startFreeTrial: boolean;
    isAdmin: boolean;
}

interface ResultVM {
    status: StatusEnum;
    message: string;
    data: any;
}

enum StatusEnum {
    Success = 1,
    Error = 2
}
