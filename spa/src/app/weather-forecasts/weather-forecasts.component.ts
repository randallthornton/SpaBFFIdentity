import { Component, Signal } from '@angular/core';
import { BackendService } from '../backend.service';
import { WeatherForecast } from '../models/weather-forecast';
import { toSignal } from '@angular/core/rxjs-interop';

@Component({
  selector: 'app-weather-forecasts',
  templateUrl: './weather-forecasts.component.html',
  styleUrls: ['./weather-forecasts.component.scss'],
  animations: [],
})
export class WeatherForecastsComponent {
  weatherForecasts: Signal<WeatherForecast[]>;

  constructor(private backendService: BackendService) {
    this.weatherForecasts = toSignal(
      this.backendService.fetchWeatherForecasts(),
      { initialValue: [] }
    );
  }
}
