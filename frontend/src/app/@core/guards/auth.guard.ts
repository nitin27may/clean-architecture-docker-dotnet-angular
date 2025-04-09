import { inject } from '@angular/core';
import { CanMatchFn, Router, CanActivateFn } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { isPlatformBrowser } from '@angular/common';
import { PLATFORM_ID } from '@angular/core';

// Helper function to check token validity
function isValidToken(token: string): boolean {
  try {
    const [, payload] = token.split('.');
    if (!payload) return false;

    const decodedPayload = JSON.parse(atob(payload));
    const currentTime = Math.floor(Date.now() / 1000);

    return decodedPayload.exp >= currentTime;
  } catch (error) {
    console.error('Token validation error:', error);
    return false;
  }
}

// Helper function to get current user from storage
function getCurrentUser() {
  const isBrowser = isPlatformBrowser(inject(PLATFORM_ID));

  if (!isBrowser) return null;

  try {
    const userStr = window.localStorage.getItem('currentUser');
    if (!userStr) return null;

    return JSON.parse(userStr);
  } catch (error) {
    console.error('User parsing error:', error);
    return null;
  }
}

// Helper function to clear user session
function clearUserSession() {
  const isBrowser = isPlatformBrowser(inject(PLATFORM_ID));

  if (isBrowser) {
    try {
      window.localStorage.removeItem('currentUser');
    } catch (error) {
      console.error('Error clearing user session:', error);
    }
  }
}

export function redirectToLoginIfNotAuthenticated(): CanMatchFn {
  return (route) => {
    const router = inject(Router);
    const snackBar = inject(MatSnackBar);
    const isBrowser = isPlatformBrowser(inject(PLATFORM_ID));

    const user = getCurrentUser();

    if (!user || !user.token || !isValidToken(user.token)) {
      // Clear invalid session
      clearUserSession();

      if (isBrowser) {
        snackBar.open('Please login to continue.', 'Close', {
          duration: 3000,
          horizontalPosition: 'end'
        });
      }

      return router.parseUrl('/login');
    }

    return true;
  };
}

export function redirectToHomeIfAuthenticated(): CanMatchFn {
  return (route) => {
    const router = inject(Router);

    const user = getCurrentUser();

    if (user && user.token && isValidToken(user.token)) {
      return router.parseUrl('/home');
    }

    return true;
  };
}

// Legacy format support (can be used with canActivate in routes)
export const AuthGuard: CanActivateFn = (route, state) => {
  const router = inject(Router);
  const snackBar = inject(MatSnackBar);
  const isBrowser = isPlatformBrowser(inject(PLATFORM_ID));

  if (!isBrowser) return false;

  const user = getCurrentUser();

  if (!user || !user.token || !isValidToken(user.token)) {
    // Clear invalid session
    clearUserSession();

    router.navigate(['/login'], {
      queryParams: { returnUrl: state.url },
      replaceUrl: true
    });

    if (isBrowser) {
      snackBar.open('Please login to continue.', 'Close', {
        duration: 3000,
        horizontalPosition: 'end'
      });
    }

    return false;
  }

  return true;
};
