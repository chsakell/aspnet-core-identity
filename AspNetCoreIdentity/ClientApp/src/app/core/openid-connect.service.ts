import { Injectable } from '@angular/core';

declare var Oidc : any;

@Injectable()
export class OpenIdConnectService {
    
    config = {
        authority: "http://localhost:5005",
        client_id: "AspNetCoreIdentity",
        redirect_uri: "http://localhost:5000",
        response_type: "code",
        scope: "openid profile SocialAPI",
        post_logout_redirect_uri: "http://localhost:5000",
    };
    userManager : any; 

    constructor() {
        this.userManager = new Oidc.UserManager(this.config);
    }

    public getUser() {
        return this.userManager.getUser();
    }

    public login() {
        return this.userManager.signinRedirect();;
    }

    public signinRedirectCallback() {
        return new Oidc.UserManager({ response_mode: "query" }).signinRedirectCallback();
    }

    public logout() {
        this.userManager.signoutRedirect();
    }

}