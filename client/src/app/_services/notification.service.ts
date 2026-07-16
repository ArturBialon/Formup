import { Injectable, inject } from '@angular/core';
import { toast } from 'ngx-sonner';
import { TranslateService } from '@ngx-translate/core';

@Injectable({
  providedIn: 'root',
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
    const actualError = error?.error && typeof error.error === 'object' ? error.error : error;
    const errorMessage = actualError?.message || actualError;

    if (typeof errorMessage === 'string' && errorMessage.includes('An unexpected server error occurred.')) return;
    if (typeof errorMessage === 'string' && errorMessage.includes('A server side error occurred.')) return;
    
    if (actualError?.errors && Array.isArray(actualError.errors)) {
    const formattedData = this.formatDataValues(actualError.data);

    actualError.errors.forEach((errorCode: string) => {
      this.showDynamicToast(errorCode, formattedData);
    });
    return;
  }

    const fallbackError = actualError?.message || 'SERVER.UNKNOWN_ERROR';
    this.showDynamicToast(fallbackError);
  }

  private showDynamicToast(errorCode: string, interpolateParams?: any): void {
    const translatedDescription = this.translate.instant(
      errorCode,
      interpolateParams
    );
    const translatedTitle = this.translate.instant('GENERAL_ERROR');

    toast.error(translatedTitle, {
      description: translatedDescription,
    });
  }

  private formatDataValues(obj: any): any {
    if (obj === null || typeof obj !== 'object') {
      return obj;
    }

    return Object.keys(obj).reduce((result: any, key: string) => {
      const value = obj[key];
      
      if (typeof value === 'number') {
        result[key] = this.currencyFormatter.format(value);
      } else {
        result[key] = value;
      }
      
      return result;
    }, {});
  }

  private currencyFormatter = new Intl.NumberFormat('pl-PL', {
    style: 'currency',
    currency: 'PLN',
    minimumFractionDigits: 2,
    maximumFractionDigits: 2
  });
}
