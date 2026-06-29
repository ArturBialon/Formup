import { Component, Input, inject, computed } from '@angular/core';
import {
  ControlContainer,
  FormControl,
  FormGroupDirective,
  ReactiveFormsModule,
} from '@angular/forms';
import { ErrorMessage } from '../../_interfaces/error-message.interface';
import { DEFAULT_ERROR_MESSAGES } from '../validators/default-error-message';
import { createMask, InputMaskModule } from '@ngneat/input-mask';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-input',
  templateUrl: './input.component.html',
  styleUrl: './input.component.css',
  standalone: true,
  imports: [ReactiveFormsModule, InputMaskModule], 
  viewProviders: [
    {
      provide: ControlContainer,
      useExisting: FormGroupDirective,
    },
  ],
})
export class InputComponent {
  private formGroupDirective = inject(FormGroupDirective);
  private translate = inject(TranslateService);

  @Input() controlName: string = '';
  @Input() label: string = '';
  @Input() placeholder = '';
  @Input() disabled: string | undefined;
  @Input() required = false;
  @Input() type = 'text';
  @Input() isMask = false;
  @Input() autocomplete: string = '';
  @Input() value: any;

  private _customErrorMessages: ErrorMessage = DEFAULT_ERROR_MESSAGES;

  @Input()
  set customErrorMessages(value: Partial<ErrorMessage>) {
    this._customErrorMessages = { ...DEFAULT_ERROR_MESSAGES, ...value };
  }

  telephone = createMask({
    mask: '999 999 999',
  });

  get getControl(): FormControl {
    return this.formGroupDirective.form.controls[
      this.controlName
    ] as FormControl;
  }

  errorMessageSignal = computed(() => {
    this.translate.currentLang(); 
    
    const control = this.formGroupDirective.form.controls[this.controlName];
    if (!control || !control.errors) return '';

    const [firstErrorKey] = Object.entries(control.errors)[0];
    
    let translationKey = '';
    if (firstErrorKey.includes('.') || firstErrorKey === firstErrorKey.toUpperCase()) {
      translationKey = firstErrorKey;
    } else {
      translationKey = this._customErrorMessages[firstErrorKey as keyof ErrorMessage] || '';
    }

    return translationKey ? this.translate.instant(translationKey) : '';
  });

  translatedLabelSignal = computed(() => {
    this.translate.currentLang();
    return this.label ? this.translate.instant(this.label) : '';
  });

  translatedPlaceholderSignal = computed(() => {
    this.translate.currentLang();
    return this.placeholder ? this.translate.instant(this.placeholder) : '';
  });

  formatAsCurrency(value: number): string {
    if (!value || isNaN(value)) {
      return value?.toString();
    }
    const currentLang = this.translate.currentLang() || 'pl-PL';
    return value.toLocaleString(currentLang, {
      minimumFractionDigits: 2,
      maximumFractionDigits: 2,
    });
  }
}