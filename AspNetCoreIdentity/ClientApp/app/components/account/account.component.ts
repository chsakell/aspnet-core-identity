import { Component, Inject } from '@angular/core';
import { Http } from '@angular/http';

@Component({
    selector: 'account',
    templateUrl: './account.component.html',
    styleUrls: ['./account.component.css']
})
export class AccountComponent {

    constructor(public http: Http, @Inject('BASE_URL') public baseUrl: string) {
        
    }
}