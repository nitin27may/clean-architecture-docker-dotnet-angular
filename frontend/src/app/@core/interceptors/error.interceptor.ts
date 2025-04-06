import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { HttpErrorResponse, HttpRequest, HttpHandlerFn } from '@angular/common/http';
import { catchError, throwError } from 'rxjs';
import { Router } from '@angular/router';
import { NotificationService } from '@core/services/notification.service';

export const ErrorInterceptor: HttpInterceptorFn = (req: HttpRequest<any>, next: HttpHandlerFn) => {
  const notificationService = inject(NotificationService);
  const router = inject(Router);

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      let errorMessage = 'An unknown error occurred!';

      if (error.status === 400) {
        errorMessage = error?.error?.message ? error.error.message : 'Bad Request.';
        notificationService.error(errorMessage);
      } else if (error.status === 401) {
        errorMessage = 'Please check your credentials and try again.';
        notificationService.error(errorMessage);
        router.navigate(['/login']);
      } else if (error.status === 404) {
        errorMessage = 'The requested resource was not found.';
        notificationService.error(errorMessage);
      } else if (error.status === 403) {
        errorMessage = `You don't have permission to access this resource.`;
        notificationService.error(errorMessage);
      } else if (error.status === 500) {
        errorMessage = 'A server-side error occurred.';
        notificationService.error(errorMessage);
      }

      return throwError(() => new Error(errorMessage));
    })
  );
};
