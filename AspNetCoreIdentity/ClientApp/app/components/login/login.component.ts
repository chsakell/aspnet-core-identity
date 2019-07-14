import { Component, Inject, OnInit } from '@angular/core';
import { Http } from '@angular/http';
import { Router } from '@angular/router';
import { StateService } from '../../core/state.service';

@Component({
    selector: 'login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
    public user: LoginVM = { username: '', password: '' }
    public errors: string = '';
    public socialProviders: Array<string> = [];

    constructor(public http: Http, 
                @Inject('BASE_URL') public baseUrl: string,
                public router: Router, public stateService: StateService) {
    }

    ngOnInit() {
        this.http.get(this.baseUrl + 'socialaccount/providers')
            .subscribe(result => {
                this.socialProviders = result.json() as Array<string>;
                console.log(this.socialProviders);
            });
    }

    login() {
        this.errors = '';
        console.log(this.user);
        this.http.post(this.baseUrl + 'api/account/login', this.user).subscribe(result => {
            let loginResult = result.json() as ResultVM;
            if (loginResult.status === StatusEnum.Success) {
                this.stateService.setAuthentication({ isAuthenticated: true, username: this.user.username });
                this.router.navigate(['/home']);
            } else if (loginResult.status === StatusEnum.Error) {
                this.errors = loginResult.data.toString();
            }

        }, error => console.error(error));
    }
}

interface LoginVM {
    username: string;
    password: string;
}

interface ResultVM {
    status: StatusEnum;
    message: string;
    data: {}
}

enum StatusEnum {
    Success = 1,
    Error = 2
}

