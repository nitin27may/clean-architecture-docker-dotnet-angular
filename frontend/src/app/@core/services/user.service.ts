import { Inject, Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { map } from "rxjs/operators";
import { User } from "../models/user.interface";
import { environment } from "../../../environments/environment";
import { DOCUMENT } from "@angular/common";

@Injectable({providedIn: 'root'})
export class UserService {
  constructor(private http: HttpClient,@Inject(DOCUMENT) private document: Document) {}

  getAll() {
    return this.http.get<User[]>(environment.apiEndpoint + "/users").pipe(
      map((users: any) => {
        return users.data;
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
  // getCurrentUser(): User {
  //   const localStorage = this.document.defaultView?.localStorage;
  //   if (localStorage && localStorage.getItem("currentUser")) {
  //     const user = JSON.parse(localStorage.getItem("currentUser"));
  //     return user;
  //   }
  // }

  create(user: User) {
    user.email = user.username;
    return this.http.post(environment.apiEndpoint + "/account/register", user);
  }

  update(user: User) {
    return this.http.put<User>(environment.apiEndpoint + "/users/" + user.id, user).pipe(
      map((user: any) => {
        return user.data;
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
}
