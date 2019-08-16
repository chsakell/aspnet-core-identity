import { Component, Inject } from '@angular/core';
import { Http } from '@angular/http';
import { StateService } from '../../core/state.service';
import { AccountDetailsVM } from '../../core/domain';

@Component({
    selector: 'manage-account',
    templateUrl: './manage-account.component.html',
})
export class ManageAccountComponent {

    public accountDetails: AccountDetailsVM = <AccountDetailsVM>{};

    constructor(public http: Http, @Inject('BASE_URL') public baseUrl: string,
        public stateService: StateService) {
        this.http.get(this.baseUrl + 'api/twoFactorAuthentication/details').subscribe(result => {
            this.accountDetails = result.json() as AccountDetailsVM;
            console.log(this.accountDetails);
        }, error => console.error(error));
    }
}