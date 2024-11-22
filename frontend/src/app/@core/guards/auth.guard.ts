import { Injectable } from '@angular/core';
import {
    ActivatedRouteSnapshot,
    Router,
    RouterStateSnapshot,
} from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard {
    constructor(private router: Router) {}

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        if (typeof window !== 'undefined') {
            if (localStorage.getItem('currentUser')) {
                // logged in so return true
                const userObject = JSON.parse(localStorage.getItem('currentUser'));

                // Extract the token
                const token = userObject.token;

                if (!token) {
                  console.error('Token is missing');
                  return this.InvalidSession(state);
                } else {
                  const payload = JSON.parse(atob(token.split('.')[1])); // Decode the payload
                  const currentTime = Math.floor(Date.now() / 1000); // Current time in seconds
                  if (payload.exp > currentTime){
                    return this.InvalidSession(state);
                  } else {
                    return true;
                }
            }
        }
        // not logged in so redirect to login page with the return url
        return this.InvalidSession(state);
    }
  }

  private InvalidSession(state: RouterStateSnapshot) {
    this.router.navigate(['/login'], {
      queryParams: { returnUrl: state.url },
    });
    return false;
  }
}
