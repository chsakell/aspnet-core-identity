import { Component, Inject } from '@angular/core';
import { Http, RequestOptions, Headers } from '@angular/http';

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

    update() {
        var categories = this.categories.filter(c => c.registered === true).map(c => c.category);
        const headers = new Headers();
        headers.append('Content-Type', 'application/json');
        let options = new RequestOptions({ headers: headers });
        this.http.post(this.baseUrl + 'api/streaming/videos/register', 
            JSON.stringify(categories), options).subscribe(result => {
            /*let registerResult = result.json() as ResultVM;
            if (registerResult.status === StatusEnum.Success) {
                this.router.navigate(['/login']);
            } else if (registerResult.status === StatusEnum.Error) {
                this.errors = registerResult.data.toString();
            }
            */
        }, error => console.error(error));
    }

}

interface StreamingCategoryVM {
    category: string;
    value: number;
    registered: boolean;
}
