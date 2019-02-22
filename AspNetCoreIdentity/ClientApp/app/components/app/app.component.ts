import { Component, Inject } from '@angular/core';
import { Http } from '@angular/http';
import { StateService, UserState } from '../../core/state.service';
import { OpenConnectIdService  } from '../../core/openconnect-id.service';

@Component({
    selector: 'app',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css']
})
export class AppComponent {
    constructor(public http: Http, stateService: StateService,
        @Inject('BASE_URL') public baseUrl: string, openConnectIdService: OpenConnectIdService) {
        this.http.get(this.baseUrl + 'api/account/authenticated').subscribe(result => {
            var state = result.json() as UserState;
            console.log(state);
            stateService.setAuthentication(state);
        }, error => console.error(error));
    }
}
