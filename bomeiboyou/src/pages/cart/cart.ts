import { Component, OnInit } from '@angular/core';
import { NavController, LoadingController, AlertController, Events } from 'ionic-angular';
import { CartItem } from '../../entity/cart-item';
import { AuthPage } from '../shared/auth';
import { ConfirmOrderPage } from '../confirm-order/confirm-order';
import { DetailPage } from '../detail/detail';
import { LogInComponent } from '../../components/log-in/log-in';
import { SignUpComponent } from '../../components/sign-up/sign-up';
import { CartService } from '../../providers/cart';
import { Context } from '../../shared/context';

@Component({
  selector: 'page-cart',
  entryComponents: [LogInComponent, SignUpComponent],
  templateUrl: 'cart.html'
})
export class CartPage extends AuthPage implements OnInit {
  public items: CartItem[];
  public isAllSelected: boolean;
  constructor(public navCtrl: NavController, public events: Events, public context: Context, public cartService: CartService,
    public loadingCtrl: LoadingController, public alertCtrl: AlertController) {
    super(navCtrl, events, cartService);
    this.items = [];
    this.isAllSelected = false;
  }

  ionViewWillEnter(): void {
    super.ionViewWillEnter();
    this.cartService.getCarts().then(data => {
      this.items = data;
      this.isReady = true;
    }); 
  }

  ionViewDidEnter(): void {
    let elem = <HTMLElement>document.querySelector("page-cart .scroll-content");
    if (elem !== null) {
        elem.style.marginBottom = this.footerHeight + 60 + "px";
    }
  }

  ngOnInit(): void {
    this.detailPage = DetailPage;
  }

  addCart(productId: string, quantity: number): void {
    this.cartService.addCart(productId, quantity);
  }

  removeCart(productId: string, quantity: number): void {
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
              this.cartService.removeCart(productId, quantity);
            }
          }
        ]
      });
      confirm.present();
    }
    else {
      this.cartService.removeCart(productId, quantity);
    }
  }

  updateCart(): void {
    this.cartService.updateCart();
  }

  checkItem(): void {
    if (this.items.length > 0) {
      this.isAllSelected = this.items.every(o => o.isSelected);
      this.cartService.updateCart();
    }
  }

  confirmOrder(): void {
    this.navCtrl.push(ConfirmOrderPage);
  }

  isSelected(): boolean {
    if (this.items.length === 0) {
      return false;
    }

    return this.items.some(o => o.isSelected);
  }

  selectAll(): void {
    let value = this.items.map(o => o.isSelected ? 1 : 0).reduce((pre, cur) => pre + cur);
    if (this.isAllSelected) {
      this.items.forEach(o => o.isSelected = true);
    }
    else if (value === 0 || value === this.items.length) {
      this.items.forEach(o => o.isSelected = this.isAllSelected);
    }
  }
}
