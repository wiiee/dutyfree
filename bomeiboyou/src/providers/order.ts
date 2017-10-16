import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import 'rxjs/add/operator/map';
import { Context } from "../shared/context";
import { BaseService } from './base';
import { ProductService } from './product';
import { OrderInfo } from '../entity/order-info';
import { CartItem } from '../entity/cart-item';

/*
  Generated class for the RegionService provider.

  See https://angular.io/docs/ts/latest/guide/dependency-injection.html
  for more info on providers and Angular 2 DI.
*/
@Injectable()
export class OrderService extends BaseService {
  orderInfos: OrderInfo[];
  constructor(public http: Http, public context: Context, public productService: ProductService) {
    super();
  }

  public payOrderInfo(orderInfoId: string): Promise<void> {
    return new Promise(resolve => {
      let url = Context.HOST + "/api/order/payOrderInfo?orderInfoId=" + orderInfoId;
      this.http.get(url, this.options).subscribe(() => {
        this.getOrderInfos(false);
        resolve();
      });
    });
  }

  public createOrderInfo(): Promise<string> {
    return new Promise(resolve => {
      let url = Context.HOST + "/api/order/CreateOrderInfo";
      this.http.get(url, this.options).map(res => res.text())
        .subscribe(data => {
          this.getOrderInfos(false);
          resolve(data);
        });
    });
  }

  public getOrderInfos(isLatest: boolean = true): Promise<OrderInfo[]> {
    if(this.orderInfos && isLatest){
      return Promise.resolve(this.orderInfos);
    }

    return new Promise(resolve => {
      let url = Context.HOST + "/api/order/getOrderInfos";
      this.http.get(url, this.options)
        .map(res => res.json())
        .subscribe(data => {
          if (data && data.length > 0) {
            let result: OrderInfo[] = [];
            data.forEach(o => result.push(new OrderInfo(o.Id, this.mapCartItems(o.ProductInfos, o.Id), o.TotalPrice, o.Status, "", "", 0)));

            let productIds = {};
            result.forEach(o => {
              o.items.forEach(p => {
                productIds[p.productId] = 1;
              });
            });

            this.productService.getProductsByIds(Object.keys(productIds)).then(data => {
              result.forEach(o => {
                o.items.forEach(p => {
                  p.logo = data[p.productId].logo;
                  p.name = data[p.productId].name;
                  p.marketPrice = data[p.productId].marketPrice;
                  p.referencePrice = data[p.productId].referencePrice;
                  p.minPrice = data[p.productId].minPrice;
                  p.maxPrice = data[p.productId].maxPrice;
                });
              });

              this.orderInfos = result;
              resolve(this.orderInfos);
            });
          }
          else {
            this.orderInfos = [];
            resolve(this.orderInfos);
          }
        });
    });
  }

  private mapCartItems(data: any, orderInfoId: string): CartItem[] {
    let result: CartItem[] = [];

    for (var key in data) {
      var element = data[key];
      result.push(new CartItem(key, element.Quantity, true, "", "", 0, element.Price, element.CommentId, orderInfoId, 0, 0, 0));
    }

    return result;
  }
  
  public removeOrderInfo(orderInfoId: string): Promise<void> {
    return new Promise(resolve => {
      let url = Context.HOST + "/api/order/removeOrderInfo?orderInfoId=" + orderInfoId;
      this.http.get(url, this.options).subscribe(() => {
        let index = this.orderInfos.findIndex(o => o.id === orderInfoId);
        this.orderInfos.splice(index, 1);
        resolve();
      });
    });
  }  
}

