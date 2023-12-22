import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class UsersService {
  url = 'api';

  constructor(private http: HttpClient) { }

  createUser(user: any) {
    return this.http.post(`${this.url}/users`, user);
  }
}
