import { Component } from '@angular/core';

declare var Oidc: any;

@Component({
    selector: 'oidc-callback',
    template: '<h1>redirecting...</h1>'
})
export class OidcCallbackComponent {
    constructor() {
        alert('say hello to my little friend!');
        //new Oidc.UserManager({ response_mode: "query" }).signinRedirectCallback().then(function () {
        //    window.location = "index.html";
        //}).catch(function (e) {
        //    console.error(e);
        //});
    }
}
