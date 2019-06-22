import { Injectable } from '@angular/core';

@Injectable()
export class StateService {
    userState: UserState = { username: '', isAuthenticated: false };

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
}

export interface UserState {
    username: string;
    isAuthenticated: boolean;
}