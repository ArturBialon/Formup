import { Injectable } from '@angular/core';
import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';
import { Router, NavigationExtras } from '@angular/router';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  constructor(private toastr: ToastrService, private router: Router) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(
      catchError((error: any) => {
        if (error instanceof HttpErrorResponse) {
          if (error.error instanceof Blob) {
            const reader = new FileReader();
            const blob = error.error;

            return new Observable<HttpEvent<unknown>>(observer => {
              reader.onload = () => {
                try {
                  const jsonResponse = JSON.parse(reader.result as string);
                  this.handleErrorResponse(jsonResponse, error.status);
                  observer.error(jsonResponse);
                } catch (e) {
                  this.toastr.error('Failed to parse error response');
                  observer.error('Failed to parse error response');
                }
              };

              reader.onerror = () => {
                this.toastr.error('Failed to read error response as Blob');
                observer.error('Failed to read error response as Blob');
              };

              reader.readAsText(blob);
            });
          } else {
            this.handleErrorResponse(error.error, error.status);
          }
        }
        return throwError(() => new Error(error));
      })
    );
  }

  private handleErrorResponse(error: any, status: number) {
    switch (status) {
      case 400:
        
        if (error && error.errors && Array.isArray(error.errors)) {
          error.errors.forEach((errorCode: string) => {
            this.toastr.error(errorCode, 'Validation Error');
          });
        } else if (error && error.error) {
          this.toastr.error(error.error, 'Error');
        } else {
          this.toastr.error('An unknown error occurred (400)', 'Error');
        }
        break;

      case 401:
        const authError = error?.error || 'AUTH.UNAUTHORIZED';
        const authDetails = error?.details || 'Session  ended or invalid credentials. Please log in again.';
        
        this.toastr.error(authDetails, authError);
        this.router.navigateByUrl('/login');
        break;

      case 403:
        const forbiddenError = error?.error || 'AUTH.FORBIDDEN';
        const forbiddenDetails = error?.details || 'Insufficient permissions for this action.';
        
        this.toastr.error(forbiddenDetails, forbiddenError);
        break;

      case 404:
        const notFoundMessage = error?.message || 'Resource not found (404)';
        this.toastr.error(notFoundMessage, 'Resource Not Found');
        break;

      case 500:
        const navigationExtras: NavigationExtras = { state: { error: error?.error || error } };
        this.router.navigateByUrl('/server-error', navigationExtras);
        break;

      default:
        this.toastr.error('An unexpected error occurred during communication.', `Status: ${status}`);
        console.error('Unhandled API Error:', error);
        break;
    }
  }
}