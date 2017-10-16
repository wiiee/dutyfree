import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import { Observable } from 'rxjs/Rx';
import 'rxjs/add/operator/map';
import 'rxjs/add/observable/forkJoin';
import { Context } from '../shared/context';
import { CartItem } from '../entity/cart-item';
import { OrderInfo } from '../entity/order-info';
import { LocalStorageKey } from '../shared/local-storage-key';
import { BaseService } from './base';
import { ProductService } from './product';
import { PartnerOrderService } from './partner-order';
import { PreferenceService } from './preference';

/*
  Generated class for the Cart provider.

  See https://angular.io/docs/ts/latest/guide/dependency-injection.html
  for more info on providers and Angular 2 DI.
*/
@Injectable()
export class CartService extends BaseService {
  //客户端购物车物品
  items: CartItem[];
  //合作端订单
  activePartnerOrderInfo: OrderInfo;
  flightNo: string;
  constructor(public http: Http, public context: Context, public productService: ProductService, public preferenceService: PreferenceService, public partnerOrderService: PartnerOrderService) {
    super();
  }

  public init(): Promise<void> {
    return new Promise(resolve => {
      Observable.forkJoin(
        this.getCarts(),
        this.getActivePartnerOrderInfo()
      ).forEach(next => {
        resolve();
      });
    });
  }

  //重置购物车，登陆、注销、生成订单的时候
  public reloadCarts(): Promise<void> {
    return new Promise(resolve => {
      //登陆、生成订单
      if (this.context.isLogIn && this.context.user.id) {
        let carts = window.localStorage.getItem(LocalStorageKey.CARTS);
        if (carts) {
          let items = JSON.parse(carts) as CartItem[];
          //清空本地购物车
          this.preferenceService.addCarts(items).then(() => {
            window.localStorage.removeItem(LocalStorageKey.CARTS);
            this.getCarts(false);
            resolve();
          });
        }
        else {
          Observable.forkJoin(
            this.getCarts(false),
            this.getActivePartnerOrderInfo(false)
          ).forEach(next => {
            resolve();
          });
        }
      }
      //注销
      else {
        this.items = [];
        this.activePartnerOrderInfo = new OrderInfo("", [], 0, 0, "", "", 0);
        resolve();
      }
    });
  }

  //获取用户的购物车
  public getCarts(isLatest: boolean = true): Promise<CartItem[]> {
    if (this.items && isLatest) {
      return Promise.resolve(this.items);
    }

    return new Promise(resolve => {
      this.preferenceService.getCarts().then(data => {
        this.items = data;
        resolve(this.items);
      });
    });
  }

  public getTotalNumber(): number {
    if (!this.context.user || this.context.user.userType === 0) {
      if (this.items.length === 0) {
        return 0;
      }

      return this.items.map(o => o.quantity).reduce((pre, cur) => pre + cur);
    }
    else {
      if (this.activePartnerOrderInfo && this.activePartnerOrderInfo.items) {
        return this.activePartnerOrderInfo.items.length;
      }
      else {
        return 0;
      }
    }
  }

  public getTotalPrice(): number {
    if (this.items.length === 0) {
      return 0;
    }

    return this.items.map(o => o.quantity * o.price).reduce((pre, cur) => pre + cur);
  }

  public getNumberSelected(): number {
    if (this.items.length === 0) {
      return 0;
    }

    return this.items.map(o => o.isSelected ? o.quantity : 0).reduce((pre, cur) => pre + cur);
  }

  public getTotalPriceSelected(): number {
    if (this.items.length === 0) {
      return 0;
    }

    return this.items.map(o => o.isSelected ? o.quantity * o.price : 0).reduce((pre, cur) => pre + cur);
  }

  public addCart(productId: string, quantity: number): void {
    var index = this.items.findIndex(o => o.productId === productId);

    //添加新物品
    if (index === -1) {
      let productIds = [productId];
      this.productService.getProductsByIds(productIds).then(data => {
        let product = data[productId];
        this.items.push(new CartItem(productId, 1, true, product.logo, product.name, product.marketPrice, product.price, null, null, 0, 0, 0));
        this.updateCart();
      });
    }
    else {
      this.items[index].quantity += quantity;
      this.updateCart();
    }
  }

  public removeCart(productId: string, quantity: number): void {
    var index = this.items.findIndex(o => o.productId === productId);

    if (index !== -1) {
      if (quantity > 0) {
        if (this.items[index].quantity > quantity) {
          this.items[index].quantity -= quantity;
        }
      }
      else {
        this.items.splice(index, 1);
      }

      this.updateCart();
    }
  }

  public updateCart(): Promise<void> {
    return this.preferenceService.updateCart(this.items);
  }

  public getActivePartnerOrderInfo(isLatest: boolean = true): Promise<OrderInfo> {
    if (this.context.isLogIn) {
      if (this.activePartnerOrderInfo && isLatest) {
        return Promise.resolve(this.activePartnerOrderInfo);
      }

      return new Promise(resolve => {
        this.partnerOrderService.getActive().then(data => {
          this.activePartnerOrderInfo = data;
          resolve(this.activePartnerOrderInfo);
        });
      });
    }
    else {
      this.activePartnerOrderInfo = new OrderInfo("", [], 0, 0, "", "", 0);
      return Promise.resolve(this.activePartnerOrderInfo);
    }
  }

  public addPartnerCart(item: CartItem, quantity?: number): Promise<void> {
    let fQuantity = quantity ? quantity : item.quantity;
    return new Promise(resolve => {
      this.partnerOrderService.addPartnerCart(item.productId, fQuantity).then(() => {
        var index = this.activePartnerOrderInfo.items.findIndex(o => o.productId === item.productId);

        //添加新物品
        if (index === -1) {
          this.activePartnerOrderInfo.items.push(item);
        }
        else {
          this.activePartnerOrderInfo.items[index].quantity += fQuantity;
        }

        resolve();
      });
    });
  }

  public removePartnerCart(productId: string, quantity: number): Promise<void> {
    return new Promise(resolve => {
      this.partnerOrderService.removePartnerCart(productId, quantity).then(() => {
        var index = this.activePartnerOrderInfo.items.findIndex(o => o.productId === productId);

        if (index !== -1) {
          if (quantity > 0) {
            if (this.activePartnerOrderInfo.items[index].quantity > quantity) {
              this.activePartnerOrderInfo.items[index].quantity -= quantity;
            }
          }
          else {
            this.activePartnerOrderInfo.items.splice(index, 1);
          }
        }

        resolve();
      });
    });
  }
}
