import { Component, Inject } from '@angular/core';
import { StateService } from '../core/state.service';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';

@Component({
    selector: 'nav-menu',
    templateUrl: './navmenu.component.html',
    styleUrls: ['./navmenu.component.css']
})
export class NavMenuComponent {
    constructor(public stateService: StateService, public router: Router,
        public http: HttpClient,
        @Inject('BASE_URL') public baseUrl: string) { }

    logout() {
        this.http.post(this.baseUrl + 'api/account/signout', {}).subscribe(result => {
            this.stateService.setAuthentication({ username: '', isAuthenticated: false, authenticationMethod: '' });
            this.router.navigate(['/home']);
        }, error => console.error(error));

    }

    closeMenu() {
        if (window.innerWidth < 768) {
            let navButton = document.getElementById('navButton');
            if (navButton) {
                navButton.click();
            }
        }
    }
}
