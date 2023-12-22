import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WeatherForecastsComponent } from './weather-forecasts.component';

describe('WeatherForecastsComponent', () => {
  let component: WeatherForecastsComponent;
  let fixture: ComponentFixture<WeatherForecastsComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [WeatherForecastsComponent]
    });
    fixture = TestBed.createComponent(WeatherForecastsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
