import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { UserVM } from '../core/domain';

@Component({
    selector: 'admin',
    templateUrl: './admin.component.html',
    styleUrls: ['./admin.component.css']
})
export class AdminComponent {
    public users: UserVM[] = [];

    constructor(public http: HttpClient, @Inject('BASE_URL') public baseUrl: string) {
        this.http.get<UserVM[]>(this.baseUrl + 'api/manage/users').subscribe(result => {
            this.users = result;
            console.log(this.users);
        }, error => console.error(error));
    }
}

