import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import 'rxjs/add/operator/map';
import { Product } from '../entity/product';
import { Page } from '../entity/page';
import { Context } from '../shared/context';
import { BaseService } from './base';

/*
  Generated class for the ProductService provider.

  See https://angular.io/docs/ts/latest/guide/dependency-injection.html
  for more info on providers and Angular 2 DI.
*/
@Injectable()
export class ProductService extends BaseService {
  constructor(public http: Http) {
    super();
  }

  public getProductsByBrand(brandId: string, page?: Page): Promise<Array<Product>> {
    return new Promise(resolve => {
      let url = Context.HOST + "/api/product/WithBrand?brandId=" + brandId;
      this.http.post(url, page && page.pageSize > 0 ? JSON.stringify(page) : null, this.options)
        .map(res => res.json())
        .subscribe(data => {
          resolve(this.getProducts(data));
        });
    });
  }

  public getProductsByIds(productIds: string[]): Promise<{ [id: string]: Product }> {
    return new Promise(resolve => {
      var url = Context.HOST + "/api/product/getProducts";
      this.http.post(url, JSON.stringify(productIds), this.options)
        .map(res => res.json())
        .subscribe(data => {
          var result = {};

          for (var key in data) {
            var element = data[key];
            result[key] = new Product(element.Id, element.Name, element.Logo, element.ImgIds,
              element.Description, element.DescriptionImgIds, element.MarketPrice, element.Price, element.ReferencePrice, element.MinPrice, element.MaxPrice);
          }

          resolve(result);
        });
    });
  }

  public getProductById(productId: string): Promise<Product> {
    return new Promise(resolve => {
      var url = Context.HOST + "/api/product/" + productId;
      this.http.get(url, this.options)
        .map(res => res.json())
        .subscribe(data => {
          if (data) {
            resolve(new Product(data.Id, data.Name, data.Logo, data.ImgIds,
              data.Description, data.DescriptionImgIds, data.MarketPrice, data.Price, data.ReferencePrice, data.MinPrice, data.MaxPrice));
          }
          else {
            resolve(null);
          }
        });
    });
  }

  private getProducts(data: any): Product[] {
    var result = [];

    data.forEach(element => {
      result.push(new Product(element.Id, element.Name, element.Logo, element.ImgIds,
        element.Description, element.DescriptionImgIds, element.MarketPrice, element.Price, element.ReferencePrice, element.MinPrice, element.MaxPrice));
    });

    return result;
  }
}