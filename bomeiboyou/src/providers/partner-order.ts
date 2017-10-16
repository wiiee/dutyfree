import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import 'rxjs/add/operator/map';
import { CartItem } from '../entity/cart-item';
import { Page } from '../entity/page';
import { OrderInfo } from '../entity/order-info';
import { Context } from '../shared/context';
import { BaseService } from './base';
import { TransferService } from './transfer';

/*
  Generated class for the PartnerOrder provider.

  See https://angular.io/docs/ts/latest/guide/dependency-injection.html
  for more info on providers and Angular 2 DI.
*/
@Injectable()
export class PartnerOrderService extends BaseService {
  constructor(public http: Http, public context: Context, public transferService: TransferService) {
    super();
  }

  //获取当前活动订单
  public getActive(): Promise<OrderInfo> {
    return new Promise(resolve => {
      let url = Context.HOST + "/api/partnerorder/getActive";
      this.http.get(url, this.options)
        .map(res => res.json())
        .subscribe(data => {
          if (data) {
            resolve(new OrderInfo(data.Id, this.mapCartItems(data.Items), 0, data.Status, data.FlightNo, data.AirportId, 1));
          }
          else {
            resolve(null);
          }
        });
    });
  }

  //获取首页上待购买的商品
  public getItems(page?: Page): Promise<CartItem[]> {
    return new Promise(resolve => {
      let url = Context.HOST + "/api/partnerorder/getProducts";
      this.http.post(url, page && page.pageSize > 0 ? JSON.stringify(page) : null, this.options)
        .map(res => res.json())
        .subscribe(data => {
          resolve(this.mapCartItems(data));
        });
    });
  }

  public setFlightNo(flightNo: string): Promise<void> {
    return new Promise(resolve => {
      let url = Context.HOST + "/api/partnerorder/setFlightNo?flightNo=" + flightNo;
      this.http.get(url, this.options).subscribe(() => {
        resolve();
      });
    });
  }

  public setAirportId(airportId: string): Promise<void> {
    return new Promise(resolve => {
      let url = Context.HOST + "/api/partnerorder/setAirportId?airportId=" + airportId;
      this.http.get(url, this.options).subscribe(() => {
        resolve();
      });
    });
  }

  public uploadPartnerOrderReceipts(imgs: string[]): Promise<void> {
    if (imgs.length === 0) {
      return Promise.resolve([]);
    }

    return new Promise(resolve => {
      let url = Context.HOST + "/api/partnerorder/uploadPartnerOrderReceipt";
      imgs.forEach((o, i) => {
        this.transferService.upload(o, url).then(data => {
          if (i === imgs.length - 1) {
            resolve();
          }
        });
      });
    });
  }

  public addPartnerCart(productId: string, quantity: number): Promise<void> {
    return new Promise(resolve => {
      let url = Context.HOST + "/api/partnerorder/AddCart?&productId=" + productId + "&quantity=" + quantity;
      this.http.get(url, this.options).subscribe(() => {
        resolve();
      });
    });
  }

  public removePartnerCart(productId: string, quantity: number): Promise<void> {
    return new Promise(resolve => {
      let url = Context.HOST + "/api/partnerorder/RemoveCart?&productId=" + productId + "&quantity=" + quantity;
      this.http.get(url, this.options).subscribe(() => {
        resolve();
      });
    });
  }

  private mapCartItems(data: Array<any>): CartItem[] {
    let result: CartItem[] = [];
    data.forEach(o => result.push(new CartItem(o.ProductId, o.Quantity, true, o.Logo, o.Name, o.MarketPrice, o.Price, "", "", o.RefrencePrice, o.MinPrice, o.MaxPrice)));
    return result;
  }
}