import { Component, OnInit } from '@angular/core';
import { NavController, ModalController } from 'ionic-angular';
import { Address } from '../../entity/address';
import { CartItem } from '../../entity/cart-item';
import { UserService } from '../../providers/user';
import { CartService } from '../../providers/cart';
import { OrderService } from '../../providers/order';
import { BasePage } from '../shared/base';
import { AddressPage } from '../address/address';
import { ProductListPage } from '../product-list/product-list';
import { AddressListPage } from '../address-list/address-list';
import { PaymentPage } from '../payment/payment';
import { DetailPage } from '../detail/detail';

/*
  Generated class for the ConfirmOrderPage page.

  See http://ionicframework.com/docs/v2/components/#navigation for more info on
  Ionic pages and navigation.
*/
@Component({
  selector: 'page-confirm-order',
  templateUrl: 'confirm-order.html'
})
export class ConfirmOrderPage extends BasePage implements OnInit {
  address: Address;
  items: CartItem[];
  isHidden: boolean = true;
  isProcessing: boolean;
  constructor(public navCtrl: NavController, public modalCtrl: ModalController, public userService: UserService,
    public cartService: CartService, public orderService: OrderService) {
    super(navCtrl, true);
    this.isProcessing = false;
    this.items = [];
  }

  ionViewWillEnter(): void {
    super.ionViewWillEnter();
    this.address = this.userService.getDefaultAddress(); 
  }

  ngOnInit(): void {
    this.detailPage = DetailPage;

    this.cartService.getCarts().then(data => {
      this.items = data;
      this.isReady = true;
    });    
  }

  addAddress(): void {
    this.navCtrl.push(AddressPage);
  }

  goProductListPage(): void {
    this.cartService.getCarts().then(data => {
      this.navCtrl.push(ProductListPage, {
        items: data.filter(o => o.isSelected)
      });
    });
  }

  goAddressListPage(): void {
    this.navCtrl.push(AddressListPage);
  }

  placeOrder(): void {
    if (!this.isProcessing) {
      this.isProcessing = true;
      this.orderService.createOrderInfo().then(data => {
        this.cartService.reloadCarts().then(() => {
          this.navCtrl.popToRoot().then(() => {
            this.navCtrl.push(PaymentPage, {
              orderInfoId: data
            });
          });
        });
      });
    }
  }

  takeSelected(items: CartItem[], index): CartItem[] {
    let itemsSelected = items.filter(o => o.isSelected);
    return super.take(itemsSelected, 3);
  }
}
