import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
} from '@angular/common/http';
import { catchError, Observable, throwError } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../services/account.service';
import { Router } from '@angular/router';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  constructor(
    private toastr: ToastrService,
    private accountService: AccountService,
    private router: Router
  ) {}

  intercept(
    request: HttpRequest<unknown>,
    next: HttpHandler
  ): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(
      catchError((error) => {
        if (error) {
          switch (error.status) {
            case 400:
              this.handle400Error(error);
              break;
            case 401:
              this.handle401Error(error);
              break;
            case 403:
              break;
            case 404:
              this.toastr.error(error.error.message, 'Not Found');
              break;
            case 500:
              this.toastr.error(error.error.message, 'Server Error');
              break;
            default:
              this.toastr.error(error.error.message, 'Unknown Error');
              break;
          }
        }
        return throwError(() => new Error(error));
      })
    );
  }

  handle400Error(error: any) {
    if (!!error.error && Array.isArray(error.error)) {
      let errorMessage = '';
      for (const key in error.error) {
        if (!!error.error[key]) {
          const errorElement = error.error[key];
          errorMessage = `${errorMessage}${errorElement.code} - ${errorElement.description}\n`;
        }
      }
      this.toastr.error(errorMessage, error.statusText);
      console.log(error.error);
    } else if (
      !!error?.error?.errors?.Content &&
      typeof error.error.errors.Content === 'object'
    ) {
      let errorObject = error.error.errors.Content;
      let errorMessage = '';
      for (const key in errorObject) {
        const errorElement = errorObject[key];
        errorMessage = `${errorMessage}${errorElement}\n`;
      }
      this.toastr.error(errorMessage, error.statusCode);
      console.log(error.error);
    } else if (!!error.error) {
      let errorMessage =
        typeof error.error === 'string'
          ? error.error
          : 'There was a validation error.';
      this.toastr.error(errorMessage, error.statusCode);
      console.log(error.error);
    } else {
      this.toastr.error(error.statusText, error.status);
      console.log(error);
    }
  }
  handle401Error(error: any) {
    let errorMessage = 'Please login to your account.';
    this.accountService.logout();
    this.toastr.error(errorMessage, error.statusCode);
    this.router.navigate(['/login']);
  }
  handle403Error(error: any) {
    let errorMessage = 'You are not authorized to access this page.';
    this.toastr.error(errorMessage, error.statusCode);
  }
  handle404Error(error: any) {
    let errorMessage = 'The requested resource was not found.';
    this.toastr.error(errorMessage, error.statusCode);
  }
  handle500Error(error: any) {
    let errorMessage = 'There was a server error.';
    this.toastr.error(errorMessage, error.statusCode);
  }
  handleUnexpectedError(error: any) {
    let errorMessage = 'An unexpected error occurred.';
    this.toastr.error(errorMessage, error.statusCode);
  }
}