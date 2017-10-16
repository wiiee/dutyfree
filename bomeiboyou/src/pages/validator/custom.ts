import { AbstractControl, ValidatorFn } from '@angular/forms';

export class CustomValidator {
  public static confirm(name: string): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any } => {
      let v = control.value;
      let e = control.root.get(name);
      
      // value not equal
      if (e && v !== e.value) return {
        confirm: false
      }

      return null;
    };
  }
}