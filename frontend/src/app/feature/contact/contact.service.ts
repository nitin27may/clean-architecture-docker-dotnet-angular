import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { catchError, map } from 'rxjs/operators';

import { throwError } from 'rxjs';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ContactService {
    constructor(private http: HttpClient) {}

    getAll() {
        return this.http.get<any[]>(environment.apiEndpoint + '/ContactPerson').pipe(
            map((res: any) => res),
            catchError(this.handleErrorObservable)
        );
    }

    getById(_id: string) {
        return this.http.get(environment.apiEndpoint + '/ContactPerson/' + _id).pipe(
            map((res: any) => res),
            catchError(this.handleErrorObservable)
        );
    }

    create(contact: any) {
        return this.http
            .post(environment.apiEndpoint + '/ContactPerson', contact)
            .pipe(
                map((res: any) => res),
                catchError(this.handleErrorObservable)
            );
    }

    update(contact: any) {
        return this.http
            .put(environment.apiEndpoint + '/ContactPerson/' + contact.id, contact)
            .pipe(
                map((res: any) => res),
                catchError(this.handleErrorObservable)
            );
    }

    delete(_id: string) {
        return this.http
            .delete(environment.apiEndpoint + '/ContactPerson/' + _id)
            .pipe(
                map((res: any) => res),
                catchError(this.handleErrorObservable)
            );
    }
    private handleErrorObservable(error: HttpErrorResponse) {
        return throwError(error);
    }
}
