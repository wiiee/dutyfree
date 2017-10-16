import { Component, OnInit } from '@angular/core';
import { NavController, AlertController } from 'ionic-angular';
import { BasePage } from '../shared/base';
import { AddressPage } from '../address/address';
import { Address } from '../../entity/address';
import { UserService } from '../../providers/user';

/*
  Generated class for the AddressListPage page.

  See http://ionicframework.com/docs/v2/components/#navigation for more info on
  Ionic pages and navigation.
*/
@Component({
  selector: 'page-address-list',
  templateUrl: 'address-list.html'
})
export class AddressListPage extends BasePage implements OnInit {
  items: Address[];
  constructor(public navCtrl: NavController, public userService: UserService, public alertCtrl: AlertController) {
    super(navCtrl, true);
  }

  ngOnInit(): void {
    this.userService.getAddresses().then(data => {
      this.items = data;
    });
  }

  addAddress(): void {
    this.navCtrl.push(AddressPage);
  }

  goAddressPage(address: Address, index: number): void {
    this.navCtrl.push(AddressPage, {
      address: address,
      index: index
    })
  }

  setDefaultAddress(index: number): void {
    this.userService.setDefaultAddress(index).then(data => {
      this.navCtrl.pop();
    });
  }

  removeAddress(index: number): void {
    let confirm = this.alertCtrl.create({
      title: '',
      message: '确定要删除该地址吗？',
      buttons: [
        {
          text: '我再想想',
          handler: () => {
          }
        },
        {
          text: '是的',
          handler: () => {
            this.userService.removeAddress(index).then(() => {
              this.userService.getAddresses().then(data => {
                this.items = data;
              });
            });
          }
        }
      ]
    });
    confirm.present();
  }
}
