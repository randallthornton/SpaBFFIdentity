import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { UsersService } from '../users.service';
import { Router } from '@angular/router';
import { RouteNames } from '../app-routing.module';

@Component({
  selector: 'app-register-user',
  templateUrl: './register-user.component.html',
  styleUrls: ['./register-user.component.scss'],
})
export class RegisterUserComponent {
  form = new FormBuilder().group({
    username: ['', Validators.required],
    email: ['', Validators.required],
    password: ['', Validators.required],
  });
  isLoading = false;

  constructor(private user: UsersService, private router: Router) {}

  onSubmitClicked() {
    this.form.markAllAsTouched();

    if (this.form.valid) {
      this.isLoading = true;
      this.user.createUser(this.form.value).subscribe(
        () => {
          this.isLoading = false;
          this.router.navigate([RouteNames.UserInfo]);
        },
        () => {
          this.isLoading = false;
        }
      );
    }
  }
}
