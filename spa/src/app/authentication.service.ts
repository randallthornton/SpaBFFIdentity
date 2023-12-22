import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, tap } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AuthenticationService {
  url = 'api/auth';
  isLoggedIn$ = new BehaviorSubject<boolean>(false);

  constructor(private http: HttpClient) {
    this.getUser().subscribe(() => {
      this.isLoggedIn$.next(true);
    });
  }

  login(username: string, password: string, persist: boolean) {
    return this.http
      .post(
        `${this.url}/login`,
        {
          username,
          password,
          persist,
        },
        {
          observe: 'response',
        }
      )
      .pipe(
        tap(() => {
          this.isLoggedIn$.next(true);
        })
      );
  }

  logout() {
    return this.http
      .post(`${this.url}/logout`, undefined)
      .pipe(tap(() => this.isLoggedIn$.next(false)));
  }

  getUser() {
    return this.http.get(`${this.url}/userInfo`);
  }
}
