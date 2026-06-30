import { HttpInterceptorFn, HttpErrorResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router, NavigationExtras } from '@angular/router';
import { catchError } from 'rxjs/operators';
import { throwError } from 'rxjs';
import { NotificationService } from '../_services/notification.service';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const router = inject(Router);
  const notifier = inject(NotificationService);

  return next(req).pipe(
    catchError((error: any) => {
      if (error instanceof HttpErrorResponse) {
        if (error.status === 500) {
          const navigationExtras: NavigationExtras = {
            state: { error: error.error },
          };
          router.navigateByUrl('/server-error', navigationExtras);
          return throwError(() => error);
        }

        if (error.status === 401) {
          router.navigateByUrl('/home');
        }

        if (error.error instanceof Blob) {
          const reader = new FileReader();
          reader.onload = () => {
            try {
              const jsonResponse = JSON.parse(reader.result as string);
              notifier.apiError(jsonResponse);
            } catch (e) {
              notifier.apiError({ errors: ['SERVER.UNKNOWN_ERROR'] });
            }
          };
          reader.readAsText(error.error);
        } else {
          notifier.apiError(error.error);
        }
      }
      return throwError(() => error);
    })
  );
};