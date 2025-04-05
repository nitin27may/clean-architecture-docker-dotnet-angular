import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { HttpErrorResponse, HttpRequest, HttpHandlerFn } from '@angular/common/http';
import { catchError, throwError } from 'rxjs';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';

export const ErrorInterceptor: HttpInterceptorFn = (req: HttpRequest<any>, next: HttpHandlerFn) => {
  const snackBar = inject(MatSnackBar);
  const router = inject(Router);

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      let errorMessage = 'An unknown error occurred!';

      if (error.status === 400) {
        errorMessage = error?.error?.message ? error.error.message : 'Bad Request.';
        snackBar.open(errorMessage, 'Close', { duration: 3000 });
      } else if (error.status === 401) {
        errorMessage = 'Please check your credentials and try again.';
        snackBar.open(errorMessage, 'Close', { duration: 3000 });
        router.navigate(['/login']);
      } else if (error.status === 404) {
        errorMessage = 'The requested resource was not found.';
        snackBar.open(errorMessage, 'Close', { duration: 3000 });
      } else if (error.status === 403) {
        errorMessage = `You don't have permission to access this resource.`;
        snackBar.open(errorMessage, 'Close', { duration: 3000 });
      } else if (error.status === 500) {
        errorMessage = 'A server-side error occurred.';
        snackBar.open(errorMessage, 'Close', { duration: 3000 });
      }

      return throwError(() => new Error(errorMessage));
    })
  );
};
