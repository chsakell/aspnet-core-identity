import { Component, Inject } from '@angular/core';
import { Http } from '@angular/http';
import { Router } from '@angular/router';

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
                public router: Router) {
    }

    login() {
        this.errors = '';
        console.log(this.user);
        this.http.post(this.baseUrl + 'api/account/login', this.user).subscribe(result => {
            let registerResult = result.json() as ResultVM;
            if (registerResult.status === StatusEnum.Success) {
                this.router.navigate(['/home']);
            } else if (registerResult.status === StatusEnum.Error) {
                this.errors = registerResult.data.toString();
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

