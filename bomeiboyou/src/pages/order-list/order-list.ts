import { Component, OnInit } from '@angular/core';
import { NavController, NavParams, AlertController } from 'ionic-angular';
import { BasePage } from '../shared/base';
import { ProductListPage } from '../product-list/product-list';
import { DetailPage } from '../detail/detail';
import { PaymentPage } from '../payment/payment';
import { OrderInfo } from '../../entity/order-info';
import { CartItem } from '../../entity/cart-item';
import { OrderService } from '../../providers/order';

/*
  Generated class for the OrderList page.

  See http://ionicframework.com/docs/v2/components/#navigation for more info on
  Ionic pages and navigation.
*/
@Component({
  selector: 'page-order-list',
  templateUrl: 'order-list.html'
})
export class OrderListPage extends BasePage implements OnInit {
  items: OrderInfo[];
  title: string;
  status: number;
  constructor(public navCtrl: NavController, public alertCtrl: AlertController, public params: NavParams, public orderService: OrderService) {
    super(navCtrl, true);
    this.status = this.params.data.status;
    this.items = [];
  }

  ionViewWillEnter(): void {
    super.ionViewWillEnter();
    this.orderService.getOrderInfos().then(data => {
      this.items = data;
    });
  }

  ngOnInit(): void {
    this.detailPage = DetailPage;

    if (this.status === 0) {
      this.title = "待付款"
    }
    else if (this.status === 1) {
      this.title = "待收货";
    }
    else if (this.status === 3) {
      this.title = "已完成";
    }
  }

  goProductListPage(items: CartItem[]): void {
    this.navCtrl.push(ProductListPage, {
      items: items
    });
  }

  goPaymentPage(orderInfoId: string): void {
    this.navCtrl.push(PaymentPage, {
      orderInfoId: orderInfoId
    });
  }

  getItems(): OrderInfo[] {
    return this.items.filter(o => o.status === this.status);
  }

  removeOrderInfo(orderInfoId: string): void {
    let confirm = this.alertCtrl.create({
      title: '',
      message: '确定要狠心移除该订单吗？',
      buttons: [
        {
          text: '我再想想',
          handler: () => {
          }
        },
        {
          text: '是的',
          handler: () => {
            this.orderService.removeOrderInfo(orderInfoId).then(() => {
              this.orderService.getOrderInfos().then(data => {
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
