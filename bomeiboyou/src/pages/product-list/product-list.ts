import { Component, OnInit } from '@angular/core';
import { NavController, NavParams } from 'ionic-angular';
import { BasePage } from '../shared/base';
import { DetailPage } from '../detail/detail';
import { PreferenceService } from '../../providers/preference';
import { CartItem } from '../../entity/cart-item';

/*
  Generated class for the ProductListPage page.

  See http://ionicframework.com/docs/v2/components/#navigation for more info on
  Ionic pages and navigation.
*/
@Component({
  selector: 'page-product-list',  
  templateUrl: 'product-list.html',
})
export class ProductListPage extends BasePage implements OnInit {
  items: CartItem[];
  constructor(public navCtrl: NavController, public params: NavParams, public preferenceService: PreferenceService) {
    super(navCtrl, true);
    this.items = this.params.data.items;
  }

  ngOnInit(): void {
    this.detailPage = DetailPage;
  }
}
