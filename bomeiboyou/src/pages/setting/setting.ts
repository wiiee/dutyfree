import { Component, OnInit } from '@angular/core';
import { NavController, Events } from 'ionic-angular';
import { BasePage } from '../shared/base';
import { UserService } from '../../providers/user';
import { CartService } from '../../providers/cart';

/*
  Generated class for the Setting page.

  See http://ionicframework.com/docs/v2/components/#navigation for more info on
  Ionic pages and navigation.
*/
@Component({
  selector: 'page-setting',
  templateUrl: 'setting.html'
})
export class SettingPage extends BasePage implements OnInit {

  constructor(public navCtrl: NavController, public userService: UserService, public cartService: CartService, public events: Events) {
    super(navCtrl, true);
  }

  ngOnInit(): void {

  }

  logOff(): void {
    this.userService.logOff().then(() => {
      this.cartService.reloadCarts().then(() => {
        this.events.publish("user:logoff");
      });
    });
  }
}
