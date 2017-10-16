import { Component, OnInit } from '@angular/core';
import { NavController, Events, Tabs } from 'ionic-angular';
import { Context } from '../../shared/context';
import { AuthPage } from '../shared/auth';
import { OrderListPage } from '../order-list/order-list';
import { WalletPage } from '../wallet/wallet';
import { MessagePage } from '../message/message';
import { QrPage } from '../qr/qr';
import { TabsPage } from '../tabs/tabs';
import { CommentListPage } from '../comment-list/comment-list';
import { SettingPage } from '../setting/setting';
import { AccountPage } from '../account/account';
import { HomePage } from '../home/home';
import { CartPage } from '../cart/cart';
import { HomePartnerPage } from '../home-partner/home-partner';
import { CartPartnerPage } from '../cart-partner/cart-partner';
import { LogInComponent } from '../../components/log-in/log-in';
import { SignUpComponent } from '../../components/sign-up/sign-up';
import { PreferenceService } from '../../providers/preference';
import { CartService } from '../../providers/cart';
import { UserService } from '../../providers/user';
import { OrderService } from '../../providers/order';
import { OrderInfo } from '../../entity/order-info';
import { Message } from '../../entity/message';
import { Wallet } from '../../entity/wallet';
import { Observable } from 'rxjs/Rx';
import 'rxjs/add/observable/forkJoin';

@Component({
  selector: 'page-information',
  entryComponents: [LogInComponent, SignUpComponent],
  templateUrl: 'information.html'
})
export class InformationPage extends AuthPage implements OnInit {
  userId: string;
  orderInfos: OrderInfo[];
  messages: Message[];
  wallet: Wallet;
  constructor(public navCtrl: NavController, public context: Context, public preferenceService: PreferenceService, public cartService: CartService,
    public userService: UserService, public orderService: OrderService, public events: Events) {
    super(navCtrl, events, cartService);
    this.orderInfos = [];
  }

  ionViewWillEnter(): void {
    super.ionViewWillEnter();

    if (this.context.isLogIn) {
      Observable.forkJoin(
        this.orderService.getOrderInfos(),
        this.preferenceService.getMessages(),
        this.preferenceService.getWallet()
      ).forEach(next => {
        this.orderInfos = next[0];
        this.messages = next[1];
        this.wallet = next[2];
      });
    }
  }

  // ionViewDidEnter(): void {
  //   if (this.pageSelector) {
  //     let elem = <HTMLElement>document.querySelector(this.pageSelector + " .scroll-content");
  //     if (elem !== null) {
  //       elem.style.marginBottom = (this.footerHeight + 60) + "px";
  //     }
  //   }
  // }

  ngOnInit(): void {

  }

  getOrderCount(status: number): number {
    if (this.orderInfos.length === 0) {
      return 0;
    }

    return this.orderInfos.filter(o => o.status === status).length;
  }

  goOrderListPage(status: number): void {
    this.navCtrl.push(OrderListPage, {
      status: status
    })
  }

  goWalletPage(): void {
    this.navCtrl.push(WalletPage, {
      wallet: this.wallet
    })
  }

  goMessagePage(): void {
    this.navCtrl.push(MessagePage, {
      messages: this.messages
    })
  }

  getPendingReviewCount(): number {
    if (this.orderInfos.length > 0) {
      return this.orderInfos.map(o => o.getPendingReviewCount()).reduce((pre, cur) => pre + cur);
    }

    return 0;
  }

  getDoneReviewCount(): number {
    if (this.orderInfos.length > 0) {
      return this.orderInfos.map(o => o.getDoneReviewCount()).reduce((pre, cur) => pre + cur);
    }

    return 0;
  }

  goQrPage(): void {
    this.navCtrl.push(QrPage);
  }

  goSettingPage(): void {
    this.navCtrl.push(SettingPage);
  }

  goAccountPage(): void {
    this.navCtrl.push(AccountPage);
  }

  goCommentListPage(isDone: boolean): void {
    let items = [];

    items = this.orderInfos.filter(o => o.status === 3);
    if (items.length > 0) {
      items = items.map(o => o.items)
        .reduce((pre, cur) => pre.concat(cur)).filter(o => isDone ? o.commentId : !o.commentId);
    }

    this.navCtrl.push(CommentListPage, {
      items: items,
      isDone: isDone
    })
  }

  selectUserType(): void {
    this.context.updateUser();
    let tabs = this.navCtrl.parent as Tabs;
    let tabsPage = tabs.viewCtrl.instance as TabsPage;

    if (this.context.user.userType === 0) {
      tabsPage.tab1Root = HomePage;
      tabsPage.tab2Root = CartPage;

      tabs.getByIndex(0).popAll().then(() => {
        tabs.getByIndex(0).setRoot(HomePage);
      });

      tabs.getByIndex(1).popAll().then(() => {
        tabs.getByIndex(1).setRoot(CartPage);
      });
    }
    else {
      tabsPage.tab1Root = HomePartnerPage;
      tabsPage.tab2Root = CartPartnerPage;

      tabs.getByIndex(0).popAll().then(() => {
        tabs.getByIndex(0).setRoot(HomePartnerPage);
      });

      tabs.getByIndex(1).popAll().then(() => {
        tabs.getByIndex(1).setRoot(CartPartnerPage);
      });
    }
  }
}