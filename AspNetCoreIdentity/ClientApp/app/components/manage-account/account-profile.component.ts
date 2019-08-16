import { Component, Input } from '@angular/core';
import { AccountDetailsVM } from '../../core/domain';

@Component({
    selector: 'account-profile',
    templateUrl: './account-profile.component.html',
    styleUrls: ['./account-profile.component.css']
})
export class AccountProfileComponent {

    @Input() accountDetails: AccountDetailsVM;

}