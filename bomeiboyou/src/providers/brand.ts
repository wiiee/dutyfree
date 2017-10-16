import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import 'rxjs/add/operator/map';
import { Brand } from "../entity/brand";
import { Context } from "../shared/context";
import { BaseService } from './base';

/*
  Generated class for the BrandService provider.

  See https://angular.io/docs/ts/latest/guide/dependency-injection.html
  for more info on providers and Angular 2 DI.
*/
@Injectable()
export class BrandService extends BaseService {
  public brandsWithCategory: any = {};
  constructor(public http: Http) {
    super();
  }

  public getBrands(categoryId: string): Promise<Array<Brand>>{
    if(this.brandsWithCategory[categoryId]){
      return Promise.resolve(this.brandsWithCategory[categoryId]);
    }

    return new Promise(resolve => {
      var url = Context.HOST + "/api/brand/GetBrands?categoryId=" + categoryId;
      this.http.get(url, this.options)
        .map(res => res.json())
        .subscribe(data => {
          var result = [];

          data.forEach(element => {
            result.push(new Brand(element.Id, element.Name.Values["zh-CN"], element.CountryId, element.Logo));
          });

          this.brandsWithCategory[categoryId] = result;
          resolve(result);
        });
    });
  }
}

