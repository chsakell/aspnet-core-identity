import { Component, Inject } from '@angular/core';
import { Http } from '@angular/http';
import { ClaimsVM, UserClaims } from '../../core/domain';

@Component({
    selector: 'claims',
    templateUrl: './claims.component.html',
    styleUrls: ['./claims.component.css']
})
export class ClaimsComponent {
    public claims: ClaimsVM[] = [];
    public userName: string = '';

    constructor(public http: Http,
        @Inject('BASE_URL') public baseUrl: string) {
        this.http.get(this.baseUrl + 'api/account/claims').subscribe(result => {
            var claimsResult = result.json() as UserClaims;
            this.claims = claimsResult.claims;
            this.userName = claimsResult.userName;
        }, error => console.error(error));
    }
}