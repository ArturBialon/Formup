import { FormArray, FormControl, FormGroup } from '@angular/forms';

export function validateAllGroupFields(formGroup: FormGroup | FormArray): void {
  Object.keys(formGroup.controls).forEach(field => {
    const control = formGroup.get(field);

    if (control instanceof FormControl) {
      control.markAllAsTouched();
      control.updateValueAndValidity();
    } else if (control instanceof FormGroup) {
      validateAllGroupFields(control);
    }
  });
}