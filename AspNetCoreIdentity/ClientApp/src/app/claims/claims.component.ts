import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ClaimsVM, UserClaims } from '../core/domain';

@Component({
    selector: 'claims',
    templateUrl: './claims.component.html',
    styleUrls: ['./claims.component.css']
})
export class ClaimsComponent {
    public claims: ClaimsVM[] = [];
    public userName: string = '';

    constructor(public http: HttpClient,
        @Inject('BASE_URL') public baseUrl: string) {
        this.http.get<UserClaims>(this.baseUrl + 'api/account/claims').subscribe(result => {
            var claimsResult = result;
            this.claims = claimsResult.claims;
            this.userName = claimsResult.userName;
        }, error => console.error(error));
    }
}
