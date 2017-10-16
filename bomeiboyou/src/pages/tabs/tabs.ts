import { Component } from '@angular/core';
import { HomePage } from '../home/home';
import { CartPage } from '../cart/cart';
import { HomePartnerPage } from '../home-partner/home-partner';
import { CartPartnerPage } from '../cart-partner/cart-partner';
import { InformationPage } from '../information/information';
import { CartService } from '../../providers/cart';
import { Context } from '../../shared/context';

@Component({
  templateUrl: 'tabs.html'
})
export class TabsPage {

  public tab1Root: any;
  public tab2Root: any;
  public tab3Root: any;

  constructor(public context: Context, public cartService: CartService) {
    // this tells the tabs component which Pages
    // should be each tab's root Pages
    if(!this.context.user || this.context.user.userType === 0){
      this.tab1Root = HomePage;
      this.tab2Root = CartPage;
      this.tab3Root = InformationPage;
    }
    else{
      this.tab1Root = HomePartnerPage;
      this.tab2Root = CartPartnerPage;
      this.tab3Root = InformationPage;
    }
  }
}
