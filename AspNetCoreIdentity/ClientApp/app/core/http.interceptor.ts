import { Injectable } from '@angular/core';
import { Request, XHRBackend, RequestOptions, Response, Http, RequestOptionsArgs, Headers } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/throw';
import { Router } from '@angular/router';
import { StateService } from './state.service';


@Injectable()
export class HttpInterceptor extends Http {

    constructor(backend: XHRBackend, defaultOptions: RequestOptions,
        public stateService: StateService, public router: Router) {
        super(backend, defaultOptions);
    }

    request(url: string | Request, options?: RequestOptionsArgs): Observable<Response> {
        return super.request(url, options).catch((error: Response) => {
            console.log(this.router.url);
            if (this.router.url == "/share")
                return Observable.throw(error);

            if ((error.status === 401) && (window.location.href.match(/\?/g) || []).length < 2) {
                this.stateService.setAuthentication({ username: '', isAuthenticated: false });
                this.router.navigate(['/login']);
            }
            else if ((error.status === 403) && (window.location.href.match(/\?/g) || []).length < 2) {
                this.router.navigate(['/access-forbidden']);
            }
            return Observable.throw(error);
        });
    }
}