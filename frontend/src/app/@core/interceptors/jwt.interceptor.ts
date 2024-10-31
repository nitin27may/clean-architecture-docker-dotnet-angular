import { HttpInterceptorFn } from '@angular/common/http';

export const JwtInterceptor: HttpInterceptorFn = (request, next) => {

  if (typeof window !== 'undefined') {
    const currentUser = JSON.parse(localStorage.getItem('currentUser'));
    if (currentUser && currentUser.token) {
      console.log('currentUser.jwToken', currentUser.token);
        request = request.clone({
            setHeaders: {
                Authorization: `Bearer ${currentUser.token}`,
            },
        });
    }
}

return next(request);
};
