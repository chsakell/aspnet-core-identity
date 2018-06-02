import { Component, Inject } from '@angular/core';
import { Http } from '@angular/http';
import {DomSanitizer, SafeUrl, SafeResourceUrl} from '@angular/platform-browser';

@Component({
    selector: 'streaming-register',
    templateUrl: './register.component.html',
    styleUrls: ['./register.component.css']
})
export class StreamingRegisterComponent {
    public categories: StreamingCategoryVM[] = [];
    public checkedAll: boolean = false;
    
    constructor(public http: Http, @Inject('BASE_URL') public baseUrl: string) {
        this.http.get(this.baseUrl + 'api/streaming/videos/register').subscribe(result => {
            this.categories = result.json() as StreamingCategoryVM[];
            console.log(this.categories);
        }, error => console.error(error));
        
    }
    
    toggleCategories($event: any) {
        var check = $event.target.checked;
        
        this.categories.forEach(c => c.registered = check);
    }

    toggleCategory(category: StreamingCategoryVM) {
        category.registered = !category.registered;

        if(!category.registered) {
            this.checkedAll = false;
        }
    }

}

interface StreamingCategoryVM {
    category: string;
    value: number;
    registered: boolean;
}
