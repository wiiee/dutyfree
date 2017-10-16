import { Component, OnInit } from '@angular/core';
import { NavController, NavParams } from 'ionic-angular';
import { BasePage } from '../shared/base';
import { Wallet } from '../../entity/wallet';

/*
  Generated class for the Wallet page.

  See http://ionicframework.com/docs/v2/components/#navigation for more info on
  Ionic pages and navigation.
*/
@Component({
  selector: 'page-wallet',
  templateUrl: 'wallet.html'
})
export class WalletPage extends BasePage implements OnInit {
  wallet: Wallet;
  constructor(public navCtrl: NavController, public params: NavParams) {
    super(navCtrl);
    this.wallet = this.params.data.wallet;
  }

  ngOnInit(): void {

  }
}
