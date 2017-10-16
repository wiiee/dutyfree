import { Component, OnInit } from '@angular/core';
import { NavController } from 'ionic-angular';
import { BasePage } from '../shared/base';
import { Context } from '../../shared/context';

/*
  Generated class for the Qr page.

  See http://ionicframework.com/docs/v2/components/#navigation for more info on
  Ionic pages and navigation.
*/
@Component({
  selector: 'page-qr',
  templateUrl: 'qr.html'
})
export class QrPage extends BasePage implements OnInit {

  constructor(public navCtrl: NavController, public context: Context) {
    super(navCtrl);
  }

  ngOnInit(): void {

  }

  getQrUrl(): string {
    return Context.HOST + "/api/user/getQr?userId=" + this.context.user.id;
  }
}
