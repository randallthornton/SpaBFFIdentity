import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { WeatherForecast } from './models/weather-forecast';
import { Post } from './models/posts';

@Injectable({
  providedIn: 'root',
})
export class BackendService {
  url = 'api';

  constructor(private http: HttpClient) { }

  fetchWeatherForecasts() {
    return this.http.get<WeatherForecast[]>(`${this.url}/weatherForecast`);
  }

  fetchWeatherForecastsSecure() {
    return this.http.get<WeatherForecast[]>(`${this.url}/secure/weatherForecast`, {
      withCredentials: true
    });
  }

  getPosts() {
    return this.http.get<Post[]>(`${this.url}/posts`);
  }

  createPost(post: any) {
    return this.http.post<Post>(`${this.url}/posts`, post);
  }

  deletePost(id: number) {
    return this.http.delete(`${this.url}/posts/${id}`);
  }

  updatePost(id: number, post: any) {
    return this.http.put(`${this.url}/posts/${id}`, post);
  }
}
