import { Component, Inject } from '@angular/core';
import { Http } from '@angular/http';
import { UserVM } from '../../core/domain';

@Component({
    selector: 'admin',
    templateUrl: './admin.component.html',
    styleUrls: ['./admin.component.css']
})
export class AdminComponent {
    public users: UserVM[] = [];
    
    constructor(public http: Http, @Inject('BASE_URL') public baseUrl: string) {
        this.http.get(this.baseUrl + 'api/manage/users').subscribe(result => {
            this.users = result.json() as UserVM[];
            console.log(this.users);
        }, error => console.error(error));
    }
}

