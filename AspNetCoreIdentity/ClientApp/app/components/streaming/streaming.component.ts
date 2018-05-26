import { Component, Inject } from '@angular/core';
import { Http } from '@angular/http';
import {DomSanitizer, SafeUrl, SafeResourceUrl} from '@angular/platform-browser';

@Component({
    selector: 'streaming',
    templateUrl: './streaming.component.html',
    styleUrls: ['./streaming.component.css']
})
export class StreamingComponent {
    public videos: VideoVM[] = [];
    
    constructor(public http: Http, public sanitizer: DomSanitizer,
        @Inject('BASE_URL') public baseUrl: string) {
        this.http.get(this.baseUrl + 'api/streaming/videos').subscribe(result => {
            this.videos = result.json() as VideoVM[];
            console.log(this.videos);
        }, error => console.error(error));
    }

    sanitizeUrl(url: string) {
        return this.sanitizer.bypassSecurityTrustResourceUrl(url);
    }
}

interface VideoVM {
    url: string;
    title: string;
    description: string;
}
