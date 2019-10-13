import { Component, Inject } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { StateService } from './core/state.service';
import { Notification } from './core/domain';
import { OpenIdConnectService } from './core/openid-connect.service';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css']
})
export class AppComponent {

    private notification: Notification = { message: '', type: '' };

    constructor(public http: HttpClient, stateService: StateService, router: Router,
        @Inject('BASE_URL') public baseUrl: string, openConnectIdService: OpenIdConnectService) {
        this.http.get<any>(this.baseUrl + 'api/account/authenticated').subscribe(result => {
            var state = result;
            console.log(state);
            stateService.setAuthentication(state);
            stateService.setDisplayPassword(state.displaySetPassword);
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

            this.notification.message = message;
            this.notification.type = type;
            stateService.displayNotification(this.notification);
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
