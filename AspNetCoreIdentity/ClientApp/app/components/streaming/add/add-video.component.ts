import { Component, Inject } from '@angular/core';
import { Http, RequestOptions, Headers } from '@angular/http';
import { VideoVM, StreamingCategoryVM } from '../../../core/domain';
import { Router } from '@angular/router';

@Component({
    selector: 'add-video',
    templateUrl: './add-video.component.html',
    styleUrls: ['./add-video.component.css']
})
export class AddVideoComponent {
    public categories: StreamingCategoryVM[] = [];
    public newVideo: VideoVM = { title : '', category : '', description: '', url : '' };
    
    constructor(public http: Http, @Inject('BASE_URL') public baseUrl: string,
    private router: Router) {
        this.http.get(this.baseUrl + 'api/streaming/videos/register').subscribe(result => {
            this.categories = result.json() as StreamingCategoryVM[];
            this.newVideo.category = this.categories[0].category;
        }, error => console.error(error));
    }

    addVideo() {
        console.log(this.newVideo);
        const headers = new Headers();
        headers.append('Content-Type', 'application/json');
        let options = new RequestOptions({ headers: headers });
        this.http.post(this.baseUrl + 'api/streaming/videos/add', 
            JSON.stringify(this.newVideo), options).subscribe(result => {
                this.router.navigate(['videos', this.newVideo.category]);
        }, error => console.error(error));
    }
}