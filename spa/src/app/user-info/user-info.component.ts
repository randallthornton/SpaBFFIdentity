import { Component } from '@angular/core';
import { AuthenticationService } from '../authentication.service';

@Component({
  selector: 'app-user-info',
  templateUrl: './user-info.component.html',
  styleUrls: ['./user-info.component.scss']
})
export class UserInfoComponent {
  isLoading = false;
  userInfo: any

  constructor(private authService: AuthenticationService) {
    this.isLoading = true;
    this.authService.getUser().subscribe((x) => {
      this.isLoading = false;
      this.userInfo = x;
    }, (err) => {
      this.isLoading = false;
    })
  }

}
