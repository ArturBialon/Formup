import { HttpInterceptorFn, HttpErrorResponse, HttpEvent } from '@angular/common/http';
import { inject } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Router, NavigationExtras } from '@angular/router';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const toastr = inject(ToastrService);
  const router = inject(Router);

  return next(req).pipe(
    catchError((error: any): Observable<HttpEvent<unknown>> => {
      if (error instanceof HttpErrorResponse) {
        if (error.error instanceof Blob) {
          return new Observable<HttpEvent<unknown>>(observer => {
            const reader = new FileReader();
            const blob = error.error;

            reader.onload = () => {
              try {
                const jsonResponse = JSON.parse(reader.result as string);
                handleErrorResponse(jsonResponse, error.status, toastr, router);
                observer.error(jsonResponse);
              } catch (e) {
                toastr.error('Failed to parse error response');
                observer.error('Failed to parse error response');
              }
            };

            reader.onerror = () => {
              toastr.error('Failed to read error response as Blob');
              observer.error('Failed to read error response as Blob');
            };

            reader.readAsText(blob);
          });
        } else {
          handleErrorResponse(error.error, error.status, toastr, router);
        }
      }
      return throwError(() => error);
    })
  );
};

function handleErrorResponse(error: any, status: number, toastr: ToastrService, router: Router) {
  switch (status) {
    case 400:
      if (error && error.errors && Array.isArray(error.errors)) {
        error.errors.forEach((errorCode: string) => {
          toastr.error(errorCode, 'Validation Error');
        });
      } else if (error && error.error) {
        toastr.error(error.error, 'Error');
      } else {
        toastr.error('An unknown error occurred (400)', 'Error');
      }
      break;

    case 401:
      const authError = error?.error || 'AUTH.UNAUTHORIZED';
      const authDetails = error?.details || 'Session ended or invalid credentials. Please log in again.';
      toastr.error(authDetails, authError);
      router.navigateByUrl('/home');
      break;

    case 403:
      const forbiddenError = error?.error || 'AUTH.FORBIDDEN';
      const forbiddenDetails = error?.details || 'Insufficient permissions for this action.';
      toastr.error(forbiddenDetails, forbiddenError);
      break;

    case 404:
      const notFoundMessage = error?.message || 'Resource not found (404)';
      toastr.error(notFoundMessage, 'Resource Not Found');
      break;

    case 500:
      const navigationExtras: NavigationExtras = { state: { error: error?.error || error } };
      router.navigateByUrl('/server-error', navigationExtras);
      break;

    default:
      toastr.error('An unexpected error occurred during communication.', `Status: ${status}`);
      console.error('Unhandled API Error:', error);
      break;
  }
}