import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { BackendService } from '../backend.service';
import { Router } from '@angular/router';
import { RouteNames } from '../app-routing.module';

@Component({
  selector: 'app-create-post',
  templateUrl: './create-post.component.html',
  styleUrls: ['./create-post.component.scss']
})
export class CreatePostComponent {
  form = new FormBuilder().group({
    title: ['', Validators.required],
    content: ['', Validators.required],
  })
  isLoading = false;

  constructor(private backendService: BackendService, private router: Router) { }

  onSubmitClicked() {
    this.form.markAllAsTouched();

    if (this.form.valid) {
      this.isLoading = true;
      this.backendService.createPost(this.form.value).subscribe(x => {
        this.isLoading = false;
        this.router.navigate([RouteNames.Posts]);
      }, (err) => {
        this.isLoading = false;
      });
    }
  }
}
