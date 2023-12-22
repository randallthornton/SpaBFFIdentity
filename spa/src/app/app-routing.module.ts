import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { WeatherForecastsComponent } from './weather-forecasts/weather-forecasts.component';
import { UserInfoComponent } from './user-info/user-info.component';
import { PostsComponent } from './posts/posts.component';
import { CreatePostComponent } from './create-post/create-post.component';
import { RegisterUserComponent } from './register-user/register-user.component';

export class RouteNames {
  static UserInfo = 'userInfo';
  static Login = 'login';
  static WeatherForecast = 'weatherForecasts';
  static Posts = 'posts';
  static CreatePost = 'createPost';
  static Register = 'register';
}

const routes: Routes = [
  {
    path: RouteNames.Login,
    component: LoginComponent,
  },
  {
    path: RouteNames.WeatherForecast,
    component: WeatherForecastsComponent,
  },
  {
    path: RouteNames.UserInfo,
    component: UserInfoComponent,
  },
  {
    path: RouteNames.Posts,
    component: PostsComponent,
  },
  {
    path: RouteNames.CreatePost,
    component: CreatePostComponent,
  },
  {
    path: RouteNames.Register,
    component: RegisterUserComponent,
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
