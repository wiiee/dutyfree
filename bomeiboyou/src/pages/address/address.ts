import { Component, OnInit } from '@angular/core';
import { NavController, NavParams, ModalController } from 'ionic-angular';
import { FormBuilder, Validators } from '@angular/forms';
import { Address } from '../../entity/address';
import { FormPage } from '../shared/form';
import { UserService } from '../../providers/user';
import { RegionPage } from '../region/region';

/*
  Generated class for the RegionPage page.

  See http://ionicframework.com/docs/v2/components/#navigation for more info on
  Ionic pages and navigation.
*/
@Component({
  selector: 'page-address',
  templateUrl: 'address.html'
})
export class AddressPage extends FormPage implements OnInit {
  address: Address;
  title: string;
  isAdd: boolean;
  index: number;
  constructor(public navCtrl: NavController, public fb: FormBuilder, public params: NavParams, public modalCtrl: ModalController,
    public userService: UserService) {
    super(navCtrl, true);

    this.formErrors = {
      'name': [],
      'phone': [],
      'text': []
    };
    this.validationMessages = {
      'name': {
        'required': '请输入姓名'
      },
      'phone': {
        'required': '请输入收货人手机号码',
        'pattern': '请输入11位手机号码'
      },
      'text': {
        'required': '请输入详细地址'
      }
    };

    if (this.params.data.address) {
      this.address = this.params.data.address;
      this.index = this.params.data.index;
      this.title = "编辑地址";
      this.isAdd = false;
    }
    else {
      this.title = "添加地址";
      this.isAdd = true;
      this.address = new Address("", "", "", [], true);
      this.address = this.userService.getDefaultAddress();
    }
  }

  onSubmit() {
    if (!this.isProcessing) {
      this.isProcessing = true;
      this.address.name = this.form.value.name;
      this.address.phone = this.form.value.phone;
      this.address.text = this.form.value.text;
      if (this.isAdd) {
        this.userService.addAddress(this.address)
          .then(() => {
            this.navCtrl.pop();
          });
      }
      else {
        this.userService.saveAddress(this.address, this.index)
          .then(() => {
            this.navCtrl.pop();
          });
      }
    }
  }

  ngOnInit(): void {
    this.buildForm();
  }

  buildForm(): void {
    this.form = this.fb.group({
      'name': [this.address.name, [
        Validators.required
      ]],
      'phone': [this.address.phone, [
        Validators.required,
        Validators.pattern("[0-9]{11}")
      ]],
      'text': [this.address.text, [
        Validators.required
      ]]
    });

    this.form.valueChanges
      .subscribe(data => this.onValueChanged(data));
  }

  selectRegion(): void {
    this.navCtrl.push(RegionPage, {
      address: this.address
    });
  }
}