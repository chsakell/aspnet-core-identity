import { Component, Inject } from '@angular/core';
import { Http } from '@angular/http';
import { DomSanitizer, SafeUrl, SafeResourceUrl } from '@angular/platform-browser';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';

@Component({
    selector: 'streaming',
    templateUrl: './streaming.component.html',
    styleUrls: ['./streaming.component.css']
})
export class StreamingComponent {
    public videos: VideoVM[] = [];
    private sub: any;

    constructor(public http: Http, public sanitizer: DomSanitizer,
        @Inject('BASE_URL') public baseUrl: string, private route: ActivatedRoute, ) {

    }

    ngOnInit() {
        this.sub = this.route.params.subscribe(params => {
            let category = params['id'];
            var route = category === undefined ? 'videos' : category
            this.http.get(this.baseUrl + `api/streaming/${route}`).subscribe(result => {
                this.videos = result.json() as VideoVM[];
                console.log(this.videos);
            }, error => console.error(error));
        });
    }

    ngOnDestroy() {
        this.sub.unsubscribe();
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
