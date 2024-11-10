import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { HttpErrorResponse, HttpRequest, HttpHandlerFn } from '@angular/common/http';
import { catchError, throwError } from 'rxjs';
import { Router } from '@angular/router';
import { ToastrService } from "ngx-toastr";

export const ErrorInterceptor: HttpInterceptorFn = (req: HttpRequest<any>, next: HttpHandlerFn) => {
  const toastService = inject(ToastrService);
  const router = inject(Router);

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      let errorMessage = 'An unknown error occurred!';

      if (error.status === 400) {
        errorMessage = error?.error?.message ? error.error.message : 'Bad Request.';
        toastService.error(errorMessage, 'Bad Request');
      } else if (error.status === 401) {
        errorMessage = 'Please check your credentials and try again.';
        toastService.error(errorMessage, 'Unauthorized');
        router.navigate(['/login']);
      } else if (error.status === 404) {
        errorMessage = 'The requested resource was not found.';
        toastService.warning(errorMessage, 'Bad Request');
      } else if (error.status === 403) {
        errorMessage = `You don't have permission to access this resource.`;
        toastService.warning(errorMessage, 'Check Permissions');
      }else if (error.status === 500) {
        errorMessage = 'A server-side error occurred.';
        toastService.error(errorMessage, 'Error');
      }

      return throwError(() => { new Error(errorMessage);

    }
    );
    })
  );
};
