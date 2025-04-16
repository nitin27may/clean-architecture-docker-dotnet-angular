import { Inject, Injectable } from "@angular/core";
import { HttpClient, HttpParams } from "@angular/common/http";
import { map } from "rxjs/operators";
import { User } from "@core/models/user.interface";

import { DOCUMENT } from "@angular/common";
import { environment } from "@environments/environment";

@Injectable({providedIn: 'root'})
export class UserService {
  constructor(private http: HttpClient,@Inject(DOCUMENT) private document: Document) {}

  getAll() {
    return this.http.get<User[]>(environment.apiEndpoint + "/users/all").pipe(
      map((users: any) => {
        return users;
      })
    );
  }

  getCurrentUser() {
    return this.http.get(environment.apiEndpoint + "/users").pipe(
      map((user: any) => {
        return user;
      })
    );
  }

  getUserWithRoles(userId: string) {
    return this.http.get(`${environment.apiEndpoint}/UserRoles/${userId}`).pipe(
      map((user: any) => {
        return user;
      })
    );
  }

  create(user: User) {
    user.email = user.userName;
    return this.http.post(environment.apiEndpoint + "/account/register", user);
  }

  update(user: User) {
    return this.http.put<User>(environment.apiEndpoint + "/users/" + user.id, user).pipe(
      map((user: any) => {
        return user;
      })
    );
  }

  getCurrentUserProfile() {
    return this.http.get<any>(environment.apiEndpoint + "/account/profile").pipe(
      map((user: any) => {
        return user;
      })
    );
  }

  changePassword(passwordDetails: any) {
    return this.http.put(environment.apiEndpoint + "/users/change-password" ,passwordDetails).pipe(map((res: any) => res.data));
  }

  delete(_id: string) {
    return this.http.delete(environment.apiEndpoint + "/user/" + _id);
  }

  getActivityLogs(username: string = '', email: string | null = null) {
    // Create params object and only add non-empty values
    let params = new HttpParams();

    if (username) {
      params = params.set('username', username);
    }

    if (email) {
      params = params.set('email', email);
    }

    return this.http.get<any[]>(`${environment.apiEndpoint}/users/activity-logs`, { params })
      .pipe(
        map(response => {
          console.log('Activity logs response:', response);
          return response;
        })
      );
  }
}
