import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { StateService } from '../core/state.service';
import { AccountDetailsVM } from '../core/domain';

@Component({
    selector: 'manage-account',
    templateUrl: './manage-account.component.html',
})
export class ManageAccountComponent {

    public accountDetails: AccountDetailsVM = <AccountDetailsVM>{};

    constructor(public http: HttpClient, @Inject('BASE_URL') public baseUrl: string,
        public stateService: StateService) {
        this.http.get<AccountDetailsVM>(this.baseUrl + 'api/twoFactorAuthentication/details').subscribe(result => {
            this.accountDetails = result;
            console.log(this.accountDetails);
        }, error => console.error(error));
    }
}
