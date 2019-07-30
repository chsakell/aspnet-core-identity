import { Component, Inject, OnInit } from '@angular/core';
import { Http } from '@angular/http';
import { Router } from '@angular/router';
import { StateService } from '../../core/state.service';

@Component({
    selector: 'password',
    templateUrl: './password.component.html',
    styleUrls: ['./password.component.css']
})
export class PasswordComponent implements OnInit {


    constructor(public http: Http,
        @Inject('BASE_URL') public baseUrl: string,
        public router: Router, public stateService: StateService) {
    }

    ngOnInit() {

    }
}
