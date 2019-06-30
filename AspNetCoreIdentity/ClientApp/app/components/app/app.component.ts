import { Component, Inject } from '@angular/core';
import { Router } from '@angular/router';
import { Http } from '@angular/http';
import { StateService, UserState } from '../../core/state.service';
import { OpenIdConnectService } from '../../core/openid-connect.service';

@Component({
    selector: 'app',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css']
})
export class AppComponent {
    constructor(public http: Http, stateService: StateService, router: Router,
        @Inject('BASE_URL') public baseUrl: string, openConnectIdService: OpenIdConnectService) {
        this.http.get(this.baseUrl + 'api/account/authenticated').subscribe(result => {
            var state = result.json() as UserState;
            console.log(state);
            stateService.setAuthentication(state);
        }, error => console.error(error));

        let code = this.getUrlParameter("code");
        if (code) {
            openConnectIdService.signinRedirectCallback().then(() => {
                router.navigate(['/share']);
            }).catch((error: any) => {
                console.error(error);
            });
        }

        let message = this.getUrlParameter("message");
        if (message) {
            let type = this.getUrlParameter("type");
            if (!type) {
                type = "success";
            }
            stateService.displayNotification({ message, type: type });
            console.log(message, type);
            if (type === "success") {
                router.navigate((['/']));
            }
        }
    }

    private getUrlParameter(name: string) {
        name = name.replace(/[\[]/, '\\[').replace(/[\]]/, '\\]');
        const regex = new RegExp('[\\?&]' + name + '=([^&#]*)');
        const results = regex.exec(location.search);
        return results === null ? '' : decodeURIComponent(results[1].replace(/\+/g, ' '));
    }
}
