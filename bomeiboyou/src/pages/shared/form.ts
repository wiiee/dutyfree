import { NavController } from 'ionic-angular';
import { FormGroup } from '@angular/forms';
import { BasePage } from './base';

export abstract class FormPage extends BasePage {
    errorMsg: string;
    form: FormGroup;
    formErrors: any = {};
    validationMessages: any = {};
    isProcessing: boolean;
    public constructor(public navCtrl: NavController, public isHideBottom = false) {
        super(navCtrl, isHideBottom);
        this.isProcessing = false;
    }

    public onValueChanged(data?: any) {
        this.errorMsg = "";
        if (!this.form) { return; }
        const form = this.form;

        for (const field in this.formErrors) {
            // clear previous error message (if any)
            this.formErrors[field] = [];
            const control = form.get(field);

            if (control && !control.valid) {
                const messages = this.validationMessages[field];
                for (const key in control.errors) {
                    if (key === "required" && control.pristine) {
                        continue;
                    }
                    else {
                        this.formErrors[field].push(messages[key]);
                    }
                }
            }
        }
    }
}