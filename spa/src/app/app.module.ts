import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input'
import { LoginComponent } from './login/login.component';
import { ReactiveFormsModule } from '@angular/forms';
import { WeatherForecastsComponent } from './weather-forecasts/weather-forecasts.component';
import { HttpClientModule } from '@angular/common/http';
import { MatMenuModule } from '@angular/material/menu';
import { MatListModule } from '@angular/material/list';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { UserInfoComponent } from './user-info/user-info.component';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { PostsComponent } from './posts/posts.component';
import { MatCardModule } from '@angular/material/card';
import { PostFormComponent } from './post-form/post-form.component';
import { CreatePostComponent } from './create-post/create-post.component';
import { RegisterUserComponent } from './register-user/register-user.component';

@NgModule({
  declarations: [AppComponent, LoginComponent, WeatherForecastsComponent, UserInfoComponent, PostsComponent, PostFormComponent, CreatePostComponent, RegisterUserComponent],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    MatToolbarModule,
    MatButtonModule,
    MatIconModule,
    MatFormFieldModule,
    ReactiveFormsModule,
    MatInputModule,
    HttpClientModule,
    MatMenuModule,
    MatListModule,
    MatCheckboxModule,
    MatProgressSpinnerModule,
    MatCardModule
  ],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule {}
