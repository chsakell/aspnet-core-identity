import { Component, Inject } from '@angular/core';
import { Http } from '@angular/http';
import { Router } from '@angular/router';
import { StateService } from '../../core/state.service';

@Component({
    selector: 'login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.css']
})
export class LoginComponent {
    public user: LoginVM = { userName: '', password: '' }
    public errors: string = '';

    constructor(public http: Http, 
                @Inject('BASE_URL') public baseUrl: string,
                public router: Router, public stateService: StateService) {
    }

    login() {
        this.errors = '';
        console.log(this.user);
        this.http.post(this.baseUrl + 'api/account/login', this.user).subscribe(result => {
            let loginResult = result.json() as ResultVM;
            if (loginResult.status === StatusEnum.Success) {
                this.stateService.setAuthentication({ isAuthenticated: true, userName: this.user.userName })
                this.router.navigate(['/home']);
            } else if (loginResult.status === StatusEnum.Error) {
                this.errors = loginResult.data.toString();
            }

        }, error => console.error(error));
    }
}

interface LoginVM {
    userName: string;
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

