import { Component, Inject, ViewContainerRef } from '@angular/core';
import { Http, RequestOptions, Headers } from '@angular/http';
import { Router } from '@angular/router';
import { ToastsManager } from 'ng2-toastr/ng2-toastr';

@Component({
    selector: 'streaming-register',
    templateUrl: './register.component.html',
    styleUrls: ['./register.component.css']
})
export class StreamingRegisterComponent {
    public categories: StreamingCategoryVM[] = [];
    public checkedAll: boolean = false;
    public displayVideoForm: boolean = false;
    
    constructor(public http: Http, @Inject('BASE_URL') public baseUrl: string,
    private router: Router,public toastr: ToastsManager, vcr: ViewContainerRef) {
        this.toastr.setRootViewContainerRef(vcr);
        this.http.get(this.baseUrl + 'api/streaming/videos/register').subscribe(result => {
            this.categories = result.json() as StreamingCategoryVM[];
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
        const headers = new Headers();
        headers.append('Content-Type', 'application/json');
        let options = new RequestOptions({ headers: headers });
        this.http.post(this.baseUrl + 'api/streaming/videos/register', 
            JSON.stringify(categories), options).subscribe(result => {
                this.toastr.success('Categories updated', 'Success!');
        }, error => console.error(error));
    }

    viewCategory(event: any, category: string) {
        event.stopPropagation();
        this.router.navigate(['/videos', category]);
    }
}

interface StreamingCategoryVM {
    category: string;
    value: number;
    registered: boolean;
}
