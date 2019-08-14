import { Component, Inject } from '@angular/core';
import { Http } from '@angular/http';
import { DomSanitizer } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';
import { VideoVM } from '../../core/domain';

@Component({
    selector: 'streaming',
    templateUrl: './streaming.component.html',
    styleUrls: ['./streaming.component.css']
})
export class StreamingComponent {
    public videos: VideoVM[] = [];
    public category: string = '';
    private sub: any;

    constructor(public http: Http, public sanitizer: DomSanitizer,
        @Inject('BASE_URL') public baseUrl: string, private route: ActivatedRoute) { }

    ngOnInit() {
        this.sub = this.route.params.subscribe(params => {
            this.category = params['id'] || '';
            var route = this.category.length === 0 ? 'videos' : this.category
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
