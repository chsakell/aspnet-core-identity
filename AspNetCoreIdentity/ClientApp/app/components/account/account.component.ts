import { Component, Inject } from '@angular/core';
import { Http } from '@angular/http';

declare var QRCode: any;

@Component({
    selector: 'account',
    templateUrl: './account.component.html',
    styleUrls: ['./account.component.css']
})
export class AccountComponent {

    public accountDetails: AccountDetailsVM = <AccountDetailsVM>{};
    public authenticatorDetails: AuthenticatorDetailsVM = <AuthenticatorDetailsVM>{};
    public displayAuthenticator: boolean = false;
    public generatingQrCode: boolean = false;

    public generatedQRCode: any;

    constructor(public http: Http, @Inject('BASE_URL') public baseUrl: string) {
        this.http.get(this.baseUrl + 'api/manageaccount/details').subscribe(result => {
            this.accountDetails = result.json() as AccountDetailsVM;
            console.log(this.accountDetails);
        }, error => console.error(error));
    }

    setupAuthenticator() {
        let self = this;
        this.http.get(this.baseUrl + 'api/manageaccount/setupAuthenticator').subscribe(result => {
            this.authenticatorDetails = result.json() as AuthenticatorDetailsVM;
            console.log(this.authenticatorDetails);
            this.displayAuthenticator = true;
            this.generatingQrCode = true;
            
            setTimeout(function () {
                self.generatedQRCode = new QRCode(document.getElementById("genQrCode"),
                    {
                        text: self.authenticatorDetails.authenticatorUri,
                        width: 150,
                        height: 150,
                        colorDark: "#000",
                        colorLight: "#ffffff",
                        correctLevel: QRCode.CorrectLevel.H
                    });
                self.generatingQrCode = false;
                (document.querySelector("#genQrCode > img") as HTMLInputElement).style.margin = "0 auto";
            },
                1000);

        }, error => console.error(error));
    }
}

interface AccountDetailsVM {
    username: string;
    email: string;
    emailConfirmed: boolean;
    phoneNumber: string;
    externalLogins: string[];
    twoFactorEnabled: boolean;
    hasAuthenticator: boolean;
    twoFactorClientRemembered: boolean;
    recoveryCodesLeft: number[];
}

interface AuthenticatorDetailsVM {
    sharedKey: string;
    authenticatorUri: string;
}