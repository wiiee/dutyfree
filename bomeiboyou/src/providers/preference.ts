import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import 'rxjs/add/operator/map';
import { LocalStorageKey } from '../shared/local-storage-key';
import { Message } from '../entity/message';
import { WalletItem } from '../entity/wallet-item';
import { Wallet } from '../entity/wallet';
import { CartItem } from '../entity/cart-item';
import { Context } from '../shared/context';
import { BaseService } from './base';
import { ProductService } from './product';
import { UserService } from './user';

/*
  Generated class for the PreferenceService provider.

  See https://angular.io/docs/ts/latest/guide/dependency-injection.html
  for more info on providers and Angular 2 DI.
*/
@Injectable()
export class PreferenceService extends BaseService {
  constructor(public http: Http, public context: Context, public productService: ProductService, public userService: UserService) {
    super();
  }

  public getMessages(): Promise<Message[]> {
    return new Promise(resolve => {
      let url = Context.HOST + "/api/preference/getMessages";
      this.http.get(url, this.options)
        .map(res => res.json())
        .subscribe(data => {
          let keys = Object.keys(data);
          if (keys.length > 0) {
            let result = [];

            keys.forEach(o => {
              let item = data[o];
              result.push(new Message(item.Content, item.IsRead, item.SenderId, new Date(item.Created)));
            });

            resolve(result);
          }
          else {
            resolve([]);
          }
        });
    });
  }

  public getWallet(): Promise<Wallet> {
    return new Promise(resolve => {
      let url = Context.HOST + "/api/preference/getWallet";
      this.http.get(url, this.options)
        .map(res => res.json())
        .subscribe(data => {
          if (data.InItems.length === 0 && data.OutItems.length === 0) {
            resolve(null);
          }
          else {
            let inItems: WalletItem[] = [];
            let outItems: WalletItem[] = [];

            if (data.InItems.length > 0) {
              data.InItems.forEach(o => inItems.push(new WalletItem(o.OrderInfoId, o.Number, o.RewardType, o.IsValid, o.Time)));
            }

            if (data.OutItems.length > 0) {
              data.OutItems.forEach(o => outItems.push(new WalletItem(o.OrderInfoId, o.Number, o.RewardType, o.IsValid, o.Time)));
            }

            resolve(new Wallet(inItems, outItems));
          }
        });
    });
  }

  //获取用户的购物车
  public getCarts(): Promise<CartItem[]> {
    return new Promise(resolve => {
      if (this.context.isLogIn && this.context.user.id) {
        let url = Context.HOST + "/api/preference/getCarts";
        this.http.get(url, this.options)
          .map(res => res.json())
          .subscribe(data => {
            resolve(this.mapCartItems(data));
          });
      } else {
        let carts = window.localStorage.getItem(LocalStorageKey.CARTS);
        let items = [];
        if (carts) {
          items = JSON.parse(carts);
        }

        resolve(items);
      }
    });
  }

  public updateCart(items: CartItem[]): Promise<void> {
    return new Promise(resolve => {
      if (this.context.isLogIn && items) {
        var url = Context.HOST + "/api/preference/updateCarts";
        this.http.post(url, JSON.stringify(items), this.options).subscribe(() => {
          resolve();
        });
      }
      else {
        window.localStorage.setItem(LocalStorageKey.CARTS, JSON.stringify(items));
        resolve();
      }
    });
  }  

  //添加物品到购物车
  public addCarts(items: CartItem[]): Promise<void> {
    return new Promise(resolve => {
      if (this.context.isLogIn && this.context.user.id) {
        let url = Context.HOST + "/api/preference/addCarts";
        this.http.post(url, JSON.stringify(items), this.options).subscribe(() => {
          resolve();
        });
      }
      else {
        resolve()
      }
    });
  }

  private mapCartItems(data: Array<any>): CartItem[] {
    let result: CartItem[] = [];
    data.forEach(o => result.push(new CartItem(o.ProductId, o.Quantity, true, o.Logo, o.Name, o.MarketPrice, o.Price, "", "", o.RefrencePrice, o.MinPrice, o.MaxPrice)));
    return result;
  }  
}