import { Component, OnInit } from '@angular/core';
import { NavController } from 'ionic-angular';
import { BasePage } from '../shared/base';

/*
  Generated class for the Account page.

  See http://ionicframework.com/docs/v2/components/#navigation for more info on
  Ionic pages and navigation.
*/
@Component({
  selector: 'page-account',
  templateUrl: 'account.html'
})
export class AccountPage extends BasePage implements OnInit {

  constructor(public navCtrl: NavController) {
    super(navCtrl, true);
  }

  ngOnInit(): void {
    
  }

}
