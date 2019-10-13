import { Component, Inject, ViewContainerRef } from '@angular/core';
import { HttpClient, HttpRequest, HttpHeaders } from '@angular/common/http';
import { Router } from '@angular/router';
import { StreamingCategoryVM } from '../../core/domain';
import { StateService } from 'src/app/core/state.service';

@Component({
    selector: 'streaming-register',
    templateUrl: './register.component.html',
    styleUrls: ['./register.component.css']
})
export class StreamingRegisterComponent {
    public categories: StreamingCategoryVM[] = [];
    public checkedAll: boolean = false;
    public displayVideoForm: boolean = false;

    constructor(public http: HttpClient, @Inject('BASE_URL') public baseUrl: string,
    private router: Router, public vcr: ViewContainerRef,
    public stateService: StateService) {
        this.http.get<StreamingCategoryVM[]>(this.baseUrl + 'api/streaming/videos/register').subscribe(result => {
            this.categories = result;
            console.log(this.categories);
        }, error => console.error(error));

    }

    toggleCategories($event: any) {
        var check = $event.target.checked;

        this.categories.forEach(c => c.registered = check);

        this.update();
    }

    toggleCategory(category: StreamingCategoryVM) {
        category.registered = !category.registered;

        if(!category.registered) {
            this.checkedAll = false;
        }

        this.update();
    }

    update() {
        var categories = this.categories.filter(c => c.registered === true).map(c => c.category);
        const headers = new HttpHeaders({'Content-Type':'application/json; charset=utf-8'});

        this.http.post(this.baseUrl + 'api/streaming/videos/register',
            JSON.stringify(categories), { headers: headers }).subscribe(result => {
                this.stateService.displayNotification({ message: 'Categories updated', type: "success" });
        }, error => console.error(error));
    }

    viewCategory(event: any, category: string) {
        event.stopPropagation();
        this.router.navigate(['/videos', category]);
    }
}
