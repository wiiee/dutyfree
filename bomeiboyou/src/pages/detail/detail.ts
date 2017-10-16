import { Component, OnInit } from '@angular/core';
import { NavController, NavParams, ToastController } from 'ionic-angular';
import { Product } from '../../entity/product';
import { Review } from '../../entity/review';
import { CartService } from '../../providers/cart';
import { ProductService } from '../../providers/product';
import { ReviewService } from '../../providers/review';
import { BasePage } from '../shared/base';
import { Observable } from 'rxjs/Rx';
import 'rxjs/add/observable/forkJoin';

/*
  Generated class for the DetailPage page.

  See http://ionicframework.com/docs/v2/components/#navigation for more info on
  Ionic pages and navigation.
*/
@Component({
  selector: 'page-detail',
  templateUrl: 'detail.html'
})
export class DetailPage extends BasePage implements OnInit {
  tab: number = 0;
  product: Product;
  review: Review;
  productId: string;
  options: any;
  public constructor(public navCtrl: NavController, public params: NavParams, public productService: ProductService, public reviewService: ReviewService, 
  public cartService: CartService, public toastCtrl: ToastController) {
    super(navCtrl);
    this.productId = this.params.data.productId;
  }

  public ngOnInit(): void {
    this.options = {
      loop: true,
      pager: true
    };

    Observable.forkJoin(
      this.productService.getProductById(this.productId),
      this.reviewService.getReview(this.productId)
    ).forEach(next => {
      this.product = next[0];
      this.review = next[1];
    });
  }

  public addCart(): void {
    this.cartService.addCart(this.product.id, 1);
    let toast = this.toastCtrl.create({
      message: '加入购物车成功',
      duration: 600,
      position: 'middle',
      cssClass: "toast-ok"
    });
    toast.present();
  }
}
