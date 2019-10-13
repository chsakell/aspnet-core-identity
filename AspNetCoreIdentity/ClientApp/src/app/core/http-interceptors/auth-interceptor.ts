import { Injectable } from '@angular/core';
import {  HttpEvent, HttpInterceptor, HttpHandler, HttpRequest } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Router } from '@angular/router';
import { StateService } from '../state.service';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
    constructor(public stateService: StateService, public router: Router) { }

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        return next.handle(request).pipe(catchError(err => {
          console.log(this.router.url);
            if (this.router.url == "/share")
                return Observable.throw(err);

                if ((err.status === 401) && (window.location.href.match(/\?/g) || []).length < 2) {
                  this.stateService.setAuthentication({ username: '', isAuthenticated: false, authenticationMethod: '' });
                  this.router.navigate(['/login']);
              }
              else if ((err.status === 403) && (window.location.href.match(/\?/g) || []).length < 2) {
                  this.router.navigate(['/access-forbidden']);
              }

            const error = err.error.message || err.statusText;
            return throwError(error);
        }))
    }
}
