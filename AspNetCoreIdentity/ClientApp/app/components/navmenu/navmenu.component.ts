import { Component, Inject } from '@angular/core';
import { StateService } from '../../core/state.service';
import { Router } from '@angular/router';
import { Http } from '@angular/http';

@Component({
    selector: 'nav-menu',
    templateUrl: './navmenu.component.html',
    styleUrls: ['./navmenu.component.css']
})
export class NavMenuComponent {
    constructor(public stateService: StateService, public router: Router,
        public http: Http,
        @Inject('BASE_URL') public baseUrl: string) { }

    logout() {
        this.http.post(this.baseUrl + 'api/account/signout', {}).subscribe(result => {
            this.stateService.setAuthentication({ username: '', isAuthenticated: false, authenticationMethod: '' });
            this.router.navigate(['/home']);
        }, error => console.error(error));

    }
}
