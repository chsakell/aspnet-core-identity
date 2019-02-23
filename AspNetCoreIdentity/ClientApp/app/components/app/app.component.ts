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

        var code = this.getUrlParameter("code");
        if (code) {
            openConnectIdService.signinRedirectCallback().then(() => {
                router.navigate(['/share']);
            }).catch((error: any) => {
                console.error(error);
            });
        }
    }

    private getUrlParameter(name: string) {
        name = name.replace(/[\[]/, '\\[').replace(/[\]]/, '\\]');
        const regex = new RegExp('[\\?&]' + name + '=([^&#]*)');
        const results = regex.exec(location.search);
        return results === null ? '' : decodeURIComponent(results[1].replace(/\+/g, ' '));
    }
}
