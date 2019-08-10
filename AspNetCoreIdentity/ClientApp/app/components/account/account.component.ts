import { Component, Inject } from '@angular/core';
import { Http } from '@angular/http';
import { StateService } from '../../core/state.service';

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
    public verificationCode: string = '';
    public errors: string = '';
    public recoveryCodes: string[] = [];
    public authenticatorNeedsSetup: boolean = false;
    public validVerificationCodes: number[] = [];
    public pollForValidVerificationCodes: any = null;
    public displayValidVerificationCodes: boolean = false;

    public generatedQRCode: any;

    constructor(public http: Http, @Inject('BASE_URL') public baseUrl: string,
        public stateService: StateService) {
        this.http.get(this.baseUrl + 'api/twoFactorAuthentication/details').subscribe(result => {
            this.accountDetails = result.json() as AccountDetailsVM;
            console.log(this.accountDetails);
        }, error => console.error(error));
    }

    setupAuthenticator() {
        let self = this;
        this.http.get(this.baseUrl + 'api/twoFactorAuthentication/setupAuthenticator').subscribe(result => {
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
                200);

        }, error => console.error(error));
    }

    toggleValidVerificationCodes(event: any) {
        let self = this;
        self.displayValidVerificationCodes = event;
        if (event) {
            self.getValidVerificationCodes();

            self.pollForValidVerificationCodes = setInterval(self.getValidVerificationCodes,
                10000);
        } else {
            this.clearValidVerificationCodes();
        }
    }

    getValidVerificationCodes() {
        this.http.get(this.baseUrl + 'api/twoFactorAuthentication/validAutheticatorCodes').subscribe(
            result => {
                this.validVerificationCodes = result.json() as number[];
            });
    }

    clearValidVerificationCodes() {
        this.displayValidVerificationCodes = false;
        this.validVerificationCodes = [];

        if (this.pollForValidVerificationCodes != null) {
            clearInterval(this.pollForValidVerificationCodes);
            this.pollForValidVerificationCodes = null;
        }
    }

    verifyAuthenticator() {
        var verification = {
            verificationCode: this.verificationCode
        };

        this.errors = '';

        this.http.post(this.baseUrl + 'api/twoFactorAuthentication/verifyAuthenticator', verification).subscribe(result => {

            let verifyAuthenticatorResult = result.json() as ResultVM;
            if (verifyAuthenticatorResult.status === StatusEnum.Success) {
                this.stateService.displayNotification({ message: verifyAuthenticatorResult.message, type: "success" });

                if (verifyAuthenticatorResult.data && verifyAuthenticatorResult.data.recoveryCodes) {
                    // display new recovery codes
                    this.recoveryCodes = verifyAuthenticatorResult.data.recoveryCodes;
                }

                this.displayAuthenticator = false;
                this.generatingQrCode = false;
                this.accountDetails.hasAuthenticator = true;
                this.accountDetails.twoFactorEnabled = true;
                this.clearValidVerificationCodes();
            } else if (verifyAuthenticatorResult.status === StatusEnum.Error) {
                this.errors = verifyAuthenticatorResult.data.toString();
            }
        },
            error => console.error(error));
    }

    onKeydown(event: any) {
        if (event.key === "Enter") {
            this.verifyAuthenticator();
        }
    }

    resetAuthenticator() {
        this.http.post(this.baseUrl + 'api/twoFactorAuthentication/resetAuthenticator', {}).subscribe(result => {

            let resetAuthenticatorResult = result.json() as ResultVM;

            if (resetAuthenticatorResult.status === StatusEnum.Success) {
                this.stateService.displayNotification({ message: resetAuthenticatorResult.message, type: "success" });
                this.accountDetails.twoFactorEnabled = false;
                this.authenticatorNeedsSetup = true;
            }
        },
            error => console.error(error));
    }

    disable2FA() {
        this.http.post(this.baseUrl + 'api/twoFactorAuthentication/disable2FA', {}).subscribe(result => {

            let disable2FAResult = result.json() as ResultVM;

            if (disable2FAResult.status === StatusEnum.Success) {
                this.stateService.displayNotification({ message: disable2FAResult.message, type: "success" });
                this.accountDetails.twoFactorEnabled = false;
            } else {
                this.stateService.displayNotification({ message: disable2FAResult.message, type: "danger" });
            }
        },
            error => console.error(error));
    }

    resetRecoverCodes() {
        this.http.post(this.baseUrl + 'api/twoFactorAuthentication/generateRecoveryCodes', {}).subscribe(result => {

            let generateRecoverCodesResult = result.json() as ResultVM;

            if (generateRecoverCodesResult.status === StatusEnum.Success) {
                this.stateService.displayNotification({ message: generateRecoverCodesResult.message, type: "success" });

                if (generateRecoverCodesResult.data && generateRecoverCodesResult.data.recoveryCodes) {
                    // display new recovery codes
                    this.recoveryCodes = generateRecoverCodesResult.data.recoveryCodes;
                }
            } else {
                this.stateService.displayNotification({ message: generateRecoverCodesResult.message, type: "danger" });
            }
        },
            error => console.error(error));
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

interface ResultVM {
    status: StatusEnum;
    message: string;
    data: any;
}

enum StatusEnum {
    Success = 1,
    Error = 2
}