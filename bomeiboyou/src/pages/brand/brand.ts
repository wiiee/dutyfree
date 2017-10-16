import { Component, OnInit } from '@angular/core';
import { NavController, NavParams, LoadingController, ToastController, InfiniteScroll } from 'ionic-angular';
import { Brand } from '../../entity/brand';
import { Product } from '../../entity/product';
import { Page } from '../../entity/page';
import { Constant } from '../../shared/constant';
import { ProductService } from '../../providers/product';
import { CartService } from '../../providers/cart';
import { BasePage } from '../shared/base';
import { DetailPage } from '../detail/detail';

/*
  Generated class for the BrandPage page.

  See http://ionicframework.com/docs/v2/components/#navigation for more info on
  Ionic pages and navigation.
*/
@Component({
  selector: 'page-brand',  
  templateUrl: 'brand.html'
})
export class BrandPage extends BasePage implements OnInit {
  brand: Brand;
  errorMsg: string;
  items: Product[];
  numbers: number[];
  groupSize: number;
  page: Page;
  offset: number = 100;
  constructor(public navCtrl: NavController, public params: NavParams, public productService: ProductService,
    public loadingCtrl: LoadingController, public toastCtrl: ToastController, public cartService: CartService) {
    super(navCtrl);
    this.brand = params.data.brand;
    this.groupSize = 2;
  }

  ngOnInit(): void {
    this.detailPage = DetailPage;
    this.items = [];
    this.numbers = [];
    this.page = new Page(0, Constant.PAGE_SIZE);

    this.productService.getProductsByBrand(this.brand.id, this.page)
      .then(data => {
        this.items = data;

        if (this.items.length === 0) {
          this.errorMsg = "暂时没有该品牌的商品在售";
        }
        else {
          this.refreshNumbers();
        }
      });
  }

  private refreshNumbers(): void {
    this.numbers = Array(Math.ceil(this.items.length / this.groupSize)).fill(1).map((x, i) => i);
  }

  public getGroup(groupIndex: number): Product[] {
    let result = [];

    for (let i = 0; i < this.groupSize; i++) {
      if (groupIndex * this.groupSize + i < this.items.length) {
        result.push(this.items[groupIndex * this.groupSize + i]);
      }
    }

    return result;
  }

  addCart(productId: string): void {
    this.cartService.addCart(productId, 1);
    let toast = this.toastCtrl.create({
      message: '加入购物车成功',
      duration: 600,
      position: 'middle',
      cssClass: "toast-ok"
    });
    toast.present();
  }

  doInfinite(infiniteScroll: InfiniteScroll): void {
    this.page.pageIndex++;
    this.productService.getProductsByBrand(this.brand.id, this.page)
      .then(data => {
        if (data.length === 0) {
          infiniteScroll.enable(false);
        }
        else {
          this.items = this.items.concat(data);
          this.refreshNumbers();
          infiniteScroll.complete();
        }
      });
  }
}