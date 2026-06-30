import { Injectable } from '@angular/core';
import { toast } from 'ngx-sonner';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {

  success(message: string): void {
    toast.success(message);
  }

  info(message: string): void {
    toast.info(message);
  }

  warning(message: string): void {
    toast.warning(message);
  }

  apiError(error: any, fallbackTitle: string = 'Error'): void {
    if (!error) {
      toast.error(fallbackTitle);
      return;
    }

    if (error.errors && Array.isArray(error.errors)) {
      error.errors.forEach((errorCode: string) => {
        toast.error(fallbackTitle, { description: errorCode });
      });
      return;
    }

    const errorDetails = error.error || error.message || null;
    if (errorDetails) {
      toast.error(fallbackTitle, { description: errorDetails });
      return;
    }

    toast.error(fallbackTitle);
  }
}