import { Component, Inject, Input } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { StateService } from '../core/state.service';
import { ResultVM, StatusEnum, AccountDetailsVM } from '../core/domain';

@Component({
    selector: 'reset-authenticator',
    templateUrl: './reset-authenticator.component.html',
    styleUrls: ['./reset-authenticator.component.css']
})
export class ResetAuthenticatorComponent {

    @Input() accountDetails: AccountDetailsVM;
    public authenticatorNeedsSetup: boolean = false;

    constructor(public http: HttpClient, @Inject('BASE_URL') public baseUrl: string,
        public stateService: StateService) {
    }

    resetAuthenticator() {
        this.http.post<ResultVM>(this.baseUrl + 'api/twoFactorAuthentication/resetAuthenticator', {}).subscribe(result => {

            let resetAuthenticatorResult = result;

            if (resetAuthenticatorResult.status === StatusEnum.Success) {
                this.stateService.displayNotification({ message: resetAuthenticatorResult.message, type: "success" });
                this.accountDetails.twoFactorEnabled = false;
                this.authenticatorNeedsSetup = true;
            }
        },
            error => console.error(error));
    }
}
