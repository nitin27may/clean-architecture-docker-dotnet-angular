import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { catchError, map } from 'rxjs/operators';
import { throwError } from 'rxjs';
import { environment } from "@environments/environment";

@Injectable({
  providedIn: 'root'
})
export class ContactService {
  private http = inject(HttpClient);
  private snackBar = inject(MatSnackBar);

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

  showNotification(message: string, isError = false) {
    this.snackBar.open(message, 'Close', {
      duration: 3000,
      horizontalPosition: 'end',
      verticalPosition: 'top',
      panelClass: isError ? ['bg-error', 'text-on-error'] : ['bg-success', 'text-on-success']
    });
  }

  private handleErrorObservable(error: any) {
    return throwError(() => error);
  }
}
