import { Component, Inject, OnInit } from '@angular/core';
import { Http } from '@angular/http';
import { Router } from '@angular/router';
import { StateService } from '../../core/state.service';

@Component({
    selector: 'password',
    templateUrl: './password.component.html',
    styleUrls: ['./password.component.css']
})
export class PasswordComponent implements OnInit {

    public errors: string = '';
    public password: string = '';
    public confirmPassword: string = '';

    constructor(public http: Http,
        @Inject('BASE_URL') public baseUrl: string,
        public router: Router, public stateService: StateService) {
    }

    ngOnInit() {

    }

    onKeydown(event: any) {
        if (event.key === "Enter") {
            this.setPassword();
        }
    }

    setPassword() {
        console.log('ok');
        this.errors = '';

        let updatePassword = {
            password: this.password,
            confirmPassword: this.confirmPassword
        };


        this.http.post(this.baseUrl + 'api/account/managepassword', updatePassword).subscribe(result => {
            let setPasswordResult = result.json() as ResultVM;

            if (setPasswordResult.status === StatusEnum.Success) {
                this.stateService.displayNotification({ message: setPasswordResult.message, type: "success" });
                this.stateService.setDisplayPassword(false);
                this.router.navigate(['/']);
            } else if (setPasswordResult.status === StatusEnum.Error) {
                this.errors = setPasswordResult.data.toString();
            }
        },
            error => console.error(error));
    }
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