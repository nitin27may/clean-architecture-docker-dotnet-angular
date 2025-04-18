import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { map } from 'rxjs/operators';

import { Router } from '@angular/router';
import { environment } from "@environments/environment";


@Injectable({providedIn: 'root'})
export class LoginService {
    constructor(
        private http: HttpClient,
        private router: Router
    ) {}

    login(username: string, password: string) {
        return this.http
            .post<any>(environment.apiEndpoint + '/users/authenticate', {
              email: username,
                username: username,
                password: password,
            })
            .pipe(
                map((user) => {
                    // login successful if there"s a jwt token in the response
                    if (user &&  user.token) {
                        // store user details and jwt token in local storage to keep user logged in between page refreshes
                        if (typeof window !== 'undefined') {
                            localStorage.setItem(
                                'currentUser',
                                JSON.stringify(user)
                            );
                        }
                    }

                    return user;
                })
            );
    }

    logout(): void {
        // remove user from local storage to log user out
        if (typeof window !== 'undefined') {
          localStorage.removeItem('currentUser');
          this.router.navigate(['login']);

        }

    }
}
