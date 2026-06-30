import { HttpInterceptorFn, HttpErrorResponse, HttpEvent } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router, NavigationExtras } from '@angular/router';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { NotificationService } from '../_services/notification.service';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const router = inject(Router);
  const notifier = inject(NotificationService);

  return next(req).pipe(
    catchError((error: any): Observable<HttpEvent<unknown>> => {
      if (error instanceof HttpErrorResponse) {
        if (error.error instanceof Blob) {
          return new Observable<HttpEvent<unknown>>(observer => {
            const reader = new FileReader();
            reader.onload = () => {
              try {
                const jsonResponse = JSON.parse(reader.result as string);
                handleError(jsonResponse, error.status, router, notifier);
                observer.error(jsonResponse);
              } catch (e) {
                notifier.apiError(null, 'Failed to parse error response');
                observer.error('Failed to parse error response');
              }
            };
            reader.readAsText(error.error);
          });
        } else {
          handleError(error.error, error.status, router, notifier);
        }
      }
      return throwError(() => error);
    })
  );
};

function handleError(error: any, status: number, router: Router, notifier: NotificationService) {
  switch (status) {
    case 400:
      notifier.apiError(error, 'Validation Error');
      break;
    case 401:
      notifier.apiError(error, 'Unauthorized');
      router.navigateByUrl('/home');
      break;
    case 403:
      notifier.apiError(error, 'Forbidden');
      break;
    case 404:
      notifier.apiError(error, 'Resource Not Found');
      break;
    case 500:
      const navigationExtras: NavigationExtras = { state: { error: error?.error || error } };
      router.navigateByUrl('/server-error', navigationExtras);
      break;
    default:
      notifier.warning('An unexpected error occurred during communication.');
      console.error('Unhandled API Error:', error);
      break;
  }
}