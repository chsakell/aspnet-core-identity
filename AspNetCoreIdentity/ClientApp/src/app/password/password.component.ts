import { Component, Inject, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { ResultVM, StatusEnum } from '../core/domain';
import { StateService } from '../core/state.service';

@Component({
    selector: 'password',
    templateUrl: './password.component.html',
    styleUrls: ['./password.component.css']
})
export class PasswordComponent implements OnInit {

    public errors: string = '';
    public password: string = '';
    public confirmPassword: string = '';

    constructor(public http: HttpClient,
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
        this.errors = '';

        let updatePassword = {
            password: this.password,
            confirmPassword: this.confirmPassword
        };


        this.http.post<ResultVM>(this.baseUrl + 'api/account/managePassword', updatePassword).subscribe(result => {
            let setPasswordResult = result;

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
