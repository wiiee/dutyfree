import { Component, OnInit } from '@angular/core';
import { NavController, Events, AlertController } from 'ionic-angular';
import { AuthPage } from '../shared/auth';
import { DetailPage } from '../detail/detail';
import { CartItem } from '../../entity/cart-item';
import { OrderInfo } from '../../entity/order-info';
import { Context } from '../../shared/context';
import { CartService } from '../../providers/cart';
import { CameraService } from '../../providers/camera';
import { PartnerOrderService } from '../../providers/partner-order';

/*
  Generated class for the CartPartner page.

  See http://ionicframework.com/docs/v2/components/#navigation for more info on
  Ionic pages and navigation.
*/
@Component({
  selector: 'page-cart-partner',
  templateUrl: 'cart-partner.html'
})
export class CartPartnerPage extends AuthPage implements OnInit {
  activePartnerOrderInfo: OrderInfo;
  items: CartItem[];
  status: number;
  imgs: string[];
  //购物车起始数量
  numbers: any;
  constructor(public navCtrl: NavController, public cartService: CartService, public events: Events, public context: Context,
    public cameraService: CameraService, public partnerOrderService: PartnerOrderService, public alertCtrl: AlertController) {
    super(navCtrl, events, cartService);
    this.imgs = [];
    this.items = [];
    this.status = 0;
    this.numbers = {};
  }

  ngOnInit(): void {
    this.detailPage = DetailPage;
    this.cartService.getActivePartnerOrderInfo().then(data => {
      if (data) {
        this.activePartnerOrderInfo = data;

        this.items = this.activePartnerOrderInfo.items;
        this.items.forEach(o => this.numbers[o.productId] = o.quantity);

        this.status = this.activePartnerOrderInfo.status;
      }
      this.isReady = true;
    });
  }

  getCount(): number {
    if (this.items.length === 0) {
      return 0;
    }

    return this.items.map(o => o.quantity).reduce((pre, cur) => pre + cur);
  }

  getPrice(): number {
    if (!this.items || this.items.length === 0) {
      return 0;
    }

    return this.items.map(o => Math.ceil(o.quantity * o.price)).reduce((pre, cur) => pre + cur);
  }

  takePhoto(): void {
    this.cameraService.takePhoto().then(data => this.imgs.push(data));
  }

  removePhoto(img: string): void {
    let index = this.imgs.indexOf(img);
    this.imgs.splice(index, 1);
  }

  uploadPhoto(): void {
    this.partnerOrderService.uploadPartnerOrderReceipts(this.imgs);
    this.status = 2;
  }

  addCart(item: CartItem, quantity: number): void {
    this.cartService.addPartnerCart(item, quantity);
  }

  removeCart(productId: string, quantity: number): void {
    if(!this.numbers[productId]){
      this.numbers[productId] = this.items.find(o => o.productId === productId).quantity;
    }

    if (quantity === 0) {
      let confirm = this.alertCtrl.create({
        title: '',
        message: '确定要狠心移除我吗？',
        buttons: [
          {
            text: '我再想想',
            handler: () => {
            }
          },
          {
            text: '是的',
            handler: () => {
              this.cartService.removePartnerCart(productId, quantity);
            }
          }
        ]
      });
      confirm.present();
    }
    else {
      this.cartService.removePartnerCart(productId, quantity);
    }
  }

  getMaxNumbers(productId: string): number {
    return this.numbers[productId] ? this.numbers[productId] : 0;
  }  
}
