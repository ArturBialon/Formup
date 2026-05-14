import { Component, Input } from '@angular/core';
import { ControlContainer, FormControl, FormGroupDirective } from '@angular/forms';
import { ErrorMessage } from 'src/app/_interfaces/error-message.interface';
import { DEFAULT_ERROR_MESSAGES } from '../validators/default-error-message';
import { createMask } from '@ngneat/input-mask';

@Component({
  selector: 'app-input',
  templateUrl: './input.component.html',
  styleUrl: './input.component.css',
  viewProviders: [
    {
      provide: ControlContainer,
      useExisting: FormGroupDirective
    }
  ],
})
export class InputComponent {
  @Input() controlName: string;
  @Input() label: string;
  @Input() placeholder = '';
  @Input() disabled: string | undefined;
  @Input() required = false;
  @Input() type = 'text';
  @Input() isMask = false;
  @Input() autocomplete: string;
  @Input() lzErrorMessage: Partial<ErrorMessage>;
  @Input() value: any;
  @Input()
  set customErrorMessages(value: Partial<ErrorMessage>) {
    this._customErrorMessages = { ...DEFAULT_ERROR_MESSAGES, ...value };
  }

  telephone = createMask({
    mask: '999 999 999',
  });

  private _customErrorMessages: ErrorMessage = DEFAULT_ERROR_MESSAGES;

  constructor(
    private formGroupDirective: FormGroupDirective
  ) { }

  get getControl(): FormControl {
    return this.formGroupDirective.form.controls[this.controlName] as FormControl;
  }

  get errorMessage(): string | void {
    const controlName = this.formGroupDirective.form.controls[this.controlName];

    if (!controlName.errors) {
      return null;
    }

    for (const [key] of Object.entries(controlName.errors)) {
      if (Object.prototype.hasOwnProperty.call(controlName.errors, key)) {
        return this.lzErrorMessage[key as keyof ErrorMessage];
      }
    }
  }

  formatAsCurrency(value: number): string {
    if (!value || isNaN(value)) {
      return value?.toString();
    }
    return value.toLocaleString('pl-PL', {
      minimumFractionDigits: 2,
      maximumFractionDigits: 2,
    });
  }
}

