import { Injectable } from '@angular/core';
import { UserState, Notification } from './domain';

@Injectable()
export class StateService {
    userState: UserState = { username: '', isAuthenticated: false, authenticationMethod: '' };
    notification: Notification = { message: '', type: '' };
    displaySetPassword: boolean = false;

    constructor() { }

    /**
     * setAuthentication
     */
    public setAuthentication(state: UserState) {
        this.userState = state;
    }

    public isAuthenticated() {
        return this.userState.isAuthenticated;
    }

    public username() {
        return this.userState.username;
    }

    public authenticationMethod() {
        return this.userState.authenticationMethod;
    }

    public displayNotification(notify: Notification) {
        this.notification.message = notify.message;
        this.notification.type = notify.type;

        setTimeout(() => {
            this.notification.message = '';
            this.notification.type = '';
        }, 8000);
    }

    public getNotification() {
        return this.notification;
    }

    public setDisplayPassword(display: boolean) {
        this.displaySetPassword = display;
    }

    public displayPassword() {
        return this.displaySetPassword;
    }
}
