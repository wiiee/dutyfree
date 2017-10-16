import { Component, OnInit } from '@angular/core';
import { NavController, NavParams } from 'ionic-angular';
import { OrderService } from '../../providers/order';
import { BasePage } from '../shared/base';

/*
  Generated class for the PaymentPage page.

  See http://ionicframework.com/docs/v2/components/#navigation for more info on
  Ionic pages and navigation.
*/
@Component({
  selector: 'page-payment',  
  templateUrl: 'payment.html',
})
export class PaymentPage extends BasePage implements OnInit {
  orderInfoId: string;
  constructor(public navCtrl: NavController, public params: NavParams, public orderService: OrderService) {
    super(navCtrl, true);
    this.orderInfoId = this.params.data.orderInfoId;
  }

  ngOnInit(): void {
    
  }
  public payOrderInfo(){
    this.orderService.payOrderInfo(this.orderInfoId);
  }
}
