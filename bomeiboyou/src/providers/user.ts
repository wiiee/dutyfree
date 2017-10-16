import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import 'rxjs/add/operator/map';
import { User } from '../entity/user';
import { ServiceResult } from '../entity/service-result';
import { Context } from '../shared/context';
import { md5 } from '../shared/md5';
import { BaseService } from './base';
import { Address } from '../entity/address';
import { Region } from '../entity/region';
import { LocalStorageKey } from '../shared/local-storage-key';

/*
  Generated class for the UserService provider.

  See https://angular.io/docs/ts/latest/guide/dependency-injection.html
  for more info on providers and Angular 2 DI.
*/
@Injectable()
export class UserService extends BaseService {
  addresses: Address[];
  constructor(public http: Http, public context: Context) {
    super();
  }

  public logIn(user: User, isEncrypt = true): Promise<ServiceResult> {
    let logInUser = new User(user.id, user.password, user.confirmPassword, user.userType);
    if (isEncrypt) {
      logInUser.password = md5(logInUser.password);
    }

    return new Promise(resolve => {
      let url = Context.HOST + "/api/user/logIn";
      this.http.post(url, JSON.stringify(logInUser), this.options)
        .map(res => res.json())
        .subscribe(data => {
          var result = new ServiceResult(data.IsSuccessful, data.Msg);

          if (data.IsSuccessful) {
            this.updateLocalData(logInUser);
          }

          resolve(result);
        });
    });
  }

  private updateLocalData(user: User): void {
    this.context.user = user;
    this.context.updateUser();
    this.context.isLogIn = true;
    window.localStorage.setItem(LocalStorageKey.LAST_LOGIN_TIME, new Date().toLocaleTimeString());
  }

  public signUp(user: User): Promise<ServiceResult> {
    let signUpUser = new User(user.id, user.password, user.confirmPassword, user.userType);
    signUpUser.password = md5(signUpUser.password);
    return new Promise(resolve => {
      let url = Context.HOST + "/api/user/signUp?deviceId=" + this.context.deviceId;
      this.http.post(url, JSON.stringify(signUpUser), this.options)
        .map(res => res.json())
        .subscribe(data => {
          var result = new ServiceResult(data.IsSuccessful, data.Msg);

          if (data.IsSuccessful) {
            this.updateLocalData(signUpUser);
          }

          resolve(result);
        });
    });
  }

  public logOff(): Promise<void> {
    return new Promise(resolve => {
      this.context.clear();
      this.context.isLogIn = false;
      this.context.user = null;
      resolve();
    });
  }

  public getDefaultAddress(): Address {
    if (this.addresses && this.addresses.length > 0) {
      return this.addresses.find(o => o.isDefault);
    }
    else {
      return new Address("", "", "", [], true);
    }
  }

  public setDefaultAddress(index: number): Promise<void> {
    return new Promise(resolve => {
      if (this.addresses && this.addresses.length > 0) {
        var pos = this.addresses.findIndex(o => o.isDefault);
        if (pos !== index) {
          let url = Context.HOST + "/api/user/SetDefaultAddress?index=" + index;
          this.http.get(url, this.options).subscribe(data => {
            this.addresses.forEach(o => o.isDefault = false);
            this.addresses[index].isDefault = true;
            resolve();
          });
        }
        else {
          resolve();
        }
      }
      else {
        resolve();
      }
    });
  }

  public getAddresses(isLatest: boolean = true): Promise<Address[]> {
    if (this.addresses && isLatest) {
      return Promise.resolve(this.addresses);
    }

    return new Promise(resolve => {
      let url = Context.HOST + "/api/user/GetAddresses";
      this.http.get(url, this.options)
        .map(res => res.json())
        .subscribe(data => {
          if (data && data.length > 0) {
            this.addresses = data.map(o => new Address(o.Name, o.Phone, o.Text,
              o.Regions.map(p => new Region(p.Id, p.Name)), o.IsDefault));
            resolve(this.addresses);
          }
          else {
            this.addresses = [];
            resolve(this.addresses);
          }
        });
    });
  }

  public addAddress(address: Address, isDefault = true): Promise<void> {
    return new Promise(resolve => {
      let url = Context.HOST + "/api/user/addAddress?isDefault=" + isDefault;
      this.http.post(url, JSON.stringify(address), this.options).subscribe(() => {
        this.addresses.forEach(o => o.isDefault = false);
        address.isDefault = true;
        this.addresses.push(address);
        resolve();
      });
    });
  }

  public saveAddress(address: Address, index: number, isDefault = true): Promise<void> {
    return new Promise(resolve => {
      let url = Context.HOST + "/api/user/saveAddress?index=" + index;
      this.http.post(url, JSON.stringify(address), this.options).subscribe(() => {
        this.addresses[index] = address;
        resolve();
      });
    });
  }

  public removeAddress(index: number): Promise<void> {
    return new Promise(resolve => {
      let url = Context.HOST + "/api/user/removeAddress?index=" + index;
      this.http.get(url, this.options).subscribe(() => {
        if (this.addresses.length === 1) {
          this.addresses = [];
        }
        else {
          let address = this.addresses[index];
          if (address.isDefault) {
            this.addresses[0].isDefault = true;
          }
          this.addresses.splice(index, 1);
        }
        resolve();
      });
    });
  }
}

