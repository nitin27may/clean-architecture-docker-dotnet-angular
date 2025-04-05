import { HttpInterceptorFn } from '@angular/common/http';

export const JwtInterceptor: HttpInterceptorFn = (request, next) => {
  if (typeof window !== 'undefined') {
    try {
      const currentUser = JSON.parse(localStorage.getItem('currentUser') || '{}');
      if (currentUser?.token) {
        request = request.clone({
          setHeaders: {
            Authorization: `Bearer ${currentUser.token}`,
          },
        });
      }
    } catch (error) {
      console.error('Error parsing currentUser from localStorage:', error);
    }
  }

  return next(request);
};
