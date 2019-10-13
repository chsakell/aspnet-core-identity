import { Injectable } from '@angular/core';
import { UserState, Notification } from './domain';

@Injectable()
export class StateService {

     public userState: UserState = { username: '', isAuthenticated: false, authenticationMethod: '' };
     public notification: Notification = { message: '', type: '' };
     public displaySetPassword: boolean = false;

    /**
     * setAuthentication
     */
    public setAuthentication(state: UserState) {
        this.userState = state;
    }

    public displayNotification(notify: Notification) {
        this.notification.message = notify.message;
        this.notification.type = notify.type;

        setTimeout(() => {
            this.notification.message = '';
            this.notification.type = '';
        }, 8000);
    }
    
    public setDisplayPassword(display: boolean) {
        this.displaySetPassword = display;
    }
}
