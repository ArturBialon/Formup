import { Injectable, inject } from '@angular/core';
import { toast } from 'ngx-sonner';
import { TranslateService } from '@ngx-translate/core';

@Injectable({
  providedIn: 'root',
})
export class NotificationService {
  private translate = inject(TranslateService);
  private formattersCache: Record<string, Intl.NumberFormat> = {};

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
    
    if (actualError?.errors) {
      const formattedData = this.formatDataValues(actualError.data);
      let errorCodesToDisplay: string[] = [];

      if (Array.isArray(actualError.errors)) {
        errorCodesToDisplay = actualError.errors;
      } 
      else if (typeof actualError.errors === 'object') {
        Object.values(actualError.errors).forEach((codes: any) => {
          if (Array.isArray(codes)) {
            errorCodesToDisplay.push(...codes);
          }
        });
      }

      const uniqueErrorCodes = [...new Set(errorCodesToDisplay)];
      
      uniqueErrorCodes.forEach((errorCode: string) => {
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

    const currencyCode = obj.currencyCode || obj.CurrencyCode || 'PLN';
    const formatter = this.getCurrencyFormatter(currencyCode);

    return Object.keys(obj).reduce((result: any, key: string) => {
      const value = obj[key];
      
      if (typeof value === 'number') {
        result[key] = formatter.format(value);
      } else {
        result[key] = value;
      }
      
      return result;
    }, {});
  }

  private getCurrencyFormatter(currencyCode: string = 'PLN'): Intl.NumberFormat {
    const cacheKey = currencyCode.toUpperCase();

    if (this.formattersCache[cacheKey]) {
      return this.formattersCache[cacheKey];
    }

    try {
      this.formattersCache[cacheKey] = new Intl.NumberFormat('pl-PL', {
        style: 'currency',
        currency: cacheKey,
        minimumFractionDigits: 2,
        maximumFractionDigits: 2
      });
    } catch (e) {
      if (!this.formattersCache['PLN']) {
        this.formattersCache['PLN'] = new Intl.NumberFormat('pl-PL', {
          style: 'currency',
          currency: 'PLN',
          minimumFractionDigits: 2,
          maximumFractionDigits: 2
        });
      }
      return this.formattersCache['PLN'];
    }

    return this.formattersCache[cacheKey];
  }
}