import { ApplicationConfig, importProvidersFrom, provideZoneChangeDetection } from '@angular/core';
import { provideRouter, withViewTransitions } from '@angular/router';
import { BrowserModule, provideClientHydration, withEventReplay } from '@angular/platform-browser';
import { provideHttpClient, withInterceptors } from "@angular/common/http";
import { JwtInterceptor } from "./@core/interceptors";
import { provideErrorTailorConfig } from "./@core/components/validation";
import { routes } from './app.routes';
import { provideNativeDateAdapter } from '@angular/material/core';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { MAT_FORM_FIELD_DEFAULT_OPTIONS } from '@angular/material/form-field';
export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    importProvidersFrom(BrowserModule),
    provideHttpClient(withInterceptors([JwtInterceptor])),
    provideRouter(routes,withViewTransitions()),
    provideClientHydration(withEventReplay()),
    provideAnimationsAsync(),
    provideNativeDateAdapter(),
    // {
    //   provide: MAT_FORM_FIELD_DEFAULT_OPTIONS,
    //   useValue: {
    //     appearance: 'outline',
    //     floatLabel: 'never',
    //     subscriptSizing: 'dynamic',
    //   },
    // },
    provideErrorTailorConfig({
      errors: {
        useFactory() {
          return {
            required: 'This field is required',
            minlength: ({ requiredLength, actualLength }) => `Expect ${requiredLength} but got ${actualLength}`,
            invalidEmailAddress: error => `Email Address is not valid`,
            invalidMobile: error => `Invalid Mobile number`,
            invalidPassword: error => `Password is weak`,
            passwordMustMatch: error => `Password is not matching`,
          };
        },
        deps: []
      }
    })],


};
