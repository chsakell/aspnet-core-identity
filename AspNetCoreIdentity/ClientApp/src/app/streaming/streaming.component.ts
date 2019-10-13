import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { DomSanitizer } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';
import { VideoVM } from '../core/domain';

@Component({
    selector: 'streaming',
    templateUrl: './streaming.component.html',
    styleUrls: ['./streaming.component.css']
})
export class StreamingComponent {
    public videos: VideoVM[] = [];
    public category: string = '';
    private sub: any;

    constructor(public http: HttpClient, public sanitizer: DomSanitizer,
        @Inject('BASE_URL') public baseUrl: string, private route: ActivatedRoute) { }

    ngOnInit() {
        this.sub = this.route.params.subscribe(params => {
            this.category = params['id'] || '';
            var route = this.category.length === 0 ? 'videos' : this.category
            this.http.get<VideoVM[]>(this.baseUrl + `api/streaming/${route}`).subscribe(result => {
                this.videos = result;
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
