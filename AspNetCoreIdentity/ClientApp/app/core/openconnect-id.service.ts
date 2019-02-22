import { Injectable } from '@angular/core';

declare var Oidc : any;

@Injectable()
export class OpenConnectIdService {
    //userState: UserState = { userName: '', isAuthenticated: false };
    config = {
        authority: "http://localhost:5005",
        client_id: "AspNetCoreIdentity",
        redirect_uri: "http://localhost:5000/callback",
        response_type: "code",
        scope: "openid profile SocialAPI",
        post_logout_redirect_uri: "http://localhost:5000",
    };
    userManager = new Oidc.UserManager(this.config);

    constructor() {
        alert('hey!');
        let self = this;
        console.log(Oidc);
        this.userManager.getUser().then(function (user: any) {
            if (user) {
                console.log("User logged in", user.profile);
                console.log(user);
            }
            else {
                console.log("User not logged in");
                self.login();
            }
        });

        //this.login();
    }

    public login() {
        return this.userManager.signinRedirect();;
    }

}

//export interface UserState {
//    userName: string;
//    isAuthenticated: boolean;
//}