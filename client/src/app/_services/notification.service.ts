import { Injectable, inject } from '@angular/core';
import { toast } from 'ngx-sonner';
import { TranslateService } from '@ngx-translate/core';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  private translate = inject(TranslateService);

  success(message: string): void {
    toast.success(this.translate.instant(message));
  }

  warning(message: string): void {
    toast.warning(this.translate.instant(message));
  }

  apiError(error: any): void {
    if (error?.errors && Array.isArray(error.errors)) {
      error.errors.forEach((errorCode: string) => {
        this.showDynamicToast(errorCode);
      });
      return;
    }

    const fallbackError = error?.error || error?.message || 'SERVER.UNKNOWN_ERROR';
    this.showDynamicToast(fallbackError);
  }

  private showDynamicToast(errorCode: string): void {
    const translatedDescription = this.translate.instant(errorCode);
    const translatedTitle = this.translate.instant('GENERAL_ERROR');
    

    toast.error(translatedTitle, {
      description: translatedDescription
    });
  }
}