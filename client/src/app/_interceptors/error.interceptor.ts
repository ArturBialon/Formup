import { Injectable } from '@angular/core';
import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr'; // Assuming you're using ngx-toastr for notifications
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

  private handleErrorResponse(error: any, status: any) {
    switch (status) {
      case 400:
        if (error.errors) {
          const modalStateErrors = [];
          for (const key in error.errors) {
            if (error.errors[key]) {
              modalStateErrors.push(error.errors[key]);
            }
          }
          this.toastr.error(modalStateErrors.flat().toString());
        } else {
          this.toastr.error(error.message, status);
        }
        break;
      case 401:
        this.toastr.error(error.error, status);
        break;
      case 403:
        this.toastr.error("Unauthorized", status);
        break;
      case 404:
        this.toastr.error(error.message, status);
        break;
      case 500:
        const navigationExtras: NavigationExtras = { state: { error: error.error } };
        this.router.navigateByUrl('/server-error', navigationExtras);
        break;
      default:
        this.toastr.error('Invalid API URL');
        console.log(error);
        break;
    }
  }
}