import { LocalStorageKey } from './local-storage-key';
import { Injectable } from '@angular/core';
import { Device } from 'ionic-native';
import { Http } from '@angular/http';
import { User } from '../entity/user';

/*
  Generated class for the CartService provider.

  See https://angular.io/docs/ts/latest/guide/dependency-injection.html
  for more info on providers and Angular 2 DI.
*/
@Injectable()
export class Context {
    //public static HOST: string = "http://localhost:55462";
    public static HOST: string = "http://120.77.1.64";
    public static HOST_STATIC: string = "http://yimeiyiyou.oss-cn-shenzhen.aliyuncs.com/pics/";
    isLogIn: boolean = false;
    user: User;
    deviceId: string;

    public constructor(private http: Http) {

    }

    public init(): Promise<void> {
        return new Promise(resolve => {
            this.deviceId = window.localStorage.getItem(LocalStorageKey.DEVICE_ID);
            if (!this.deviceId) {
                window.localStorage.clear();
                this.deviceId = Device.device.uuid;
                let url = Context.HOST + "/api/user/initDevice?deviceId=" + this.deviceId;
                this.http.get(url).subscribe();
                window.localStorage.setItem(LocalStorageKey.DEVICE_ID, this.deviceId);
            }
            this.user = JSON.parse(window.localStorage.getItem(LocalStorageKey.USER));
            if (this.user) {
                this.isLogIn = true;
            }
            resolve();
        });
    }

    public updateUser(): void {
        window.localStorage.setItem(LocalStorageKey.USER, JSON.stringify(this.user));
    }

    public clear(): void {
        window.localStorage.removeItem(LocalStorageKey.USER);
        window.localStorage.removeItem(LocalStorageKey.LAST_LOGIN_TIME);
        window.localStorage.removeItem(LocalStorageKey.CARTS);
    }
}