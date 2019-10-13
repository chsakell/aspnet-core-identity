export interface UserVM {
    id: string;
    email: string;
    emailConfirmed: boolean;
    userName: string;
    lockoutEnabled: boolean;
    twoFactorEnabled: boolean;
}

export interface ClaimsVM {
    type: string;
    value: string;
}

export interface UserClaims {
    claims: ClaimsVM[];
    userName: string;
}

export interface LoginVM {
    username: string;
    password: string;
}

export interface RegisterVM {
    userName: string;
    email: string;
    password: string;
    confirmPassword: string;
    startFreeTrial: boolean;
    isAdmin: boolean;
}

export interface IContact {
    id: string;
    name: string;
    username: string;
    email: string;
}

export interface StreamingCategoryVM {
    category: string;
    value: number;
    registered: boolean;
}

export interface VideoVM {
    url: string;
    title: string;
    description: string;
    category: string;
}

export interface UserState {
    username: string;
    isAuthenticated: boolean;
    authenticationMethod: string;
}

export interface Notification {
    message: string;
    type: string;
}

export interface AccountDetailsVM {
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

export interface AuthenticatorDetailsVM {
    sharedKey: string;
    authenticatorUri: string;
}

export interface ResultVM {
    status: StatusEnum;
    message: string;
    data: any;
}

export enum StatusEnum {
    Success = 1,
    Error = 2
}
