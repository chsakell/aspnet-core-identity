import { Component, Inject } from '@angular/core';
import { Http, Headers, RequestOptions } from '@angular/http';
import { Router } from '@angular/router';
import { StateService } from '../../core/state.service';
import { OpenConnectIdService } from '../../core/openconnect-id.service';

@Component({
    selector: 'social-api-share',
    templateUrl: './share.component.html',
    styleUrls: ['./share.component.css']
})
export class SocialApiShareComponent {

    public socialLoggedIn: any;
    public contacts: IContact[] = [];

    constructor(public http: Http,
        public openConnectIdService: OpenConnectIdService,
        public router: Router, public stateService: StateService) {
        openConnectIdService.getUser().then((user: any) => {
            if (user) {
                console.log("User logged in", user.profile);
                console.log(user);
                this.socialLoggedIn = true;

                const headers = new Headers();
                headers.append("Authorization", `Bearer ${user.access_token}`);

                const options = new RequestOptions({ headers: headers });

                const socialApiContactsURI = "http://localhost:5010/api/contacts";

                this.http.get(socialApiContactsURI, options).subscribe(result => {
                    this.contacts = result.json() as IContact[];

                }, error => console.error(error));
            }

        });
    }

    login() {
        this.openConnectIdService.login();
    }

    logout() {
        this.openConnectIdService.logout();
    }
}

interface IContact {
    id: string;
    name: string;
    username: string;
    email: string;
}