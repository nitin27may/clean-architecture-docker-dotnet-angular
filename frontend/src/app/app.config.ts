import { ApplicationConfig, importProvidersFrom, provideZoneChangeDetection } from '@angular/core';
import { provideAnimations } from '@angular/platform-browser/animations';
import { provideRouter } from '@angular/router';

import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { BrowserModule, provideClientHydration } from '@angular/platform-browser';
import { provideToastr } from 'ngx-toastr';
import { AppRoutingModule, routes } from './app.routes';
import { provideErrorTailorConfig } from "./@core/components/validation";
import { ErrorInterceptor, JwtInterceptor } from "./@core/interceptors";
import { error } from "console";

export const appConfig: ApplicationConfig = {
    providers: [
        importProvidersFrom(BrowserModule, AppRoutingModule),
        provideHttpClient(),
        provideZoneChangeDetection({ eventCoalescing: true }),
        provideHttpClient(withInterceptors([JwtInterceptor, ErrorInterceptor])),
        provideRouter(routes),
        provideClientHydration(),
        provideAnimations(), // required animations providers
        provideToastr(), // Toastr providers
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
          //controlErrorComponent: CustomControlErrorComponent, // Uncomment to see errors being rendered using a custom component
          //controlErrorComponentAnchorFn: controlErrorComponentAnchorFn // Uncomment to see errors being positioned differently
        })
    ],
};
