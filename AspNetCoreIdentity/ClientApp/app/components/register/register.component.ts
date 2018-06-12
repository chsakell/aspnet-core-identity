import { Component, Inject } from '@angular/core';
import { Http } from '@angular/http';
import { Router } from '@angular/router';

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

    constructor(public http: Http,
        @Inject('BASE_URL') public baseUrl: string,
        public router: Router) {
    }

    register() {
        this.errors = '';
        this.http.post(this.baseUrl + 'api/account/register', this.user).subscribe(result => {
            let registerResult = result.json() as ResultVM;
            if (registerResult.status === StatusEnum.Success) {
                this.router.navigate(['/login']);
            } else if (registerResult.status === StatusEnum.Error) {
                this.errors = registerResult.data.toString();
            }

        }, error => console.error(error));
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

    isSimpleUser(event: any) {
        this.simpleUser = event;
        if (event) {
            this.user.isAdmin = false;
            this.user.startFreeTrial = false;
        }
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
    data: {}
}

enum StatusEnum {
    Success = 1,
    Error = 2
}
