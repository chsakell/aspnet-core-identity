import { Component, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { VideoVM, StreamingCategoryVM } from '../../core/domain';
import { Router } from '@angular/router';

@Component({
  selector: 'add-video',
  templateUrl: './add-video.component.html',
  styleUrls: ['./add-video.component.css']
})
export class AddVideoComponent {
  public categories: StreamingCategoryVM[] = [];
  public newVideo: VideoVM = { title: '', category: '', description: '', url: '' };

  constructor(public http: HttpClient, @Inject('BASE_URL') public baseUrl: string,
    private router: Router) {
    this.http.get<StreamingCategoryVM[]>(this.baseUrl + 'api/streaming/videos/register').subscribe(result => {
      this.categories = result;
      this.newVideo.category = this.categories[0].category;
    }, error => console.error(error));
  }

  addVideo() {
    let categoryId = this.categories.find(cat => cat.category === this.newVideo.category).value;
    console.log(this.newVideo);
    var postData = {
      title: this.newVideo.title,
      category: categoryId,
      description: this.newVideo.description,
      url: this.newVideo.url
    };
    this.http.post(this.baseUrl + 'api/streaming/videos/add', postData).subscribe(result => {
      this.router.navigate(['videos', this.newVideo.category]);
    }, error => console.error(error));
  }
}
