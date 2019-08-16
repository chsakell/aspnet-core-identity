import { Component, Inject, Input } from '@angular/core';
import { Http } from '@angular/http';
import { StateService } from '../../core/state.service';
import { ResultVM, StatusEnum, AccountDetailsVM } from '../../core/domain';

@Component({
    selector: 'reset-authenticator',
    templateUrl: './reset-authenticator.component.html',
    styleUrls: ['./reset-authenticator.component.css']
})
export class ResetAuthenticatorComponent {

    @Input() accountDetails: AccountDetailsVM;
    public authenticatorNeedsSetup: boolean = false;

    constructor(public http: Http, @Inject('BASE_URL') public baseUrl: string,
        public stateService: StateService) {
    }

    resetAuthenticator() {
        this.http.post(this.baseUrl + 'api/twoFactorAuthentication/resetAuthenticator', {}).subscribe(result => {

            let resetAuthenticatorResult = result.json() as ResultVM;

            if (resetAuthenticatorResult.status === StatusEnum.Success) {
                this.stateService.displayNotification({ message: resetAuthenticatorResult.message, type: "success" });
                this.accountDetails.twoFactorEnabled = false;
                this.authenticatorNeedsSetup = true;
            }
        },
            error => console.error(error));
    }
}