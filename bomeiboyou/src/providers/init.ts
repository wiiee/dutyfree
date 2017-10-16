import { Injectable } from '@angular/core';
import { Context } from '../shared/context';
import { UserService } from './user';
import { CartService } from './cart';
import { GeoService } from './geo';

/*
  Generated class for the Init provider.

  See https://angular.io/docs/ts/latest/guide/dependency-injection.html
  for more info on providers and Angular 2 DI.
*/
@Injectable()
export class InitService {

  constructor(public context: Context, public userService: UserService, public cartService: CartService, public geoService: GeoService) {

  }

  init(): Promise<void> {
    return new Promise(resolve => {
      this.context.init()
        .then(() => {
          this.geoService.getLocation();
          if (this.context.isLogIn) {
            this.userService.logIn(this.context.user, false).then(result => {
              this.userService.getAddresses();
              this.cartService.init().then(() => resolve());
            });
          }
          else {
            this.cartService.init().then(() => resolve());
          }
        });
    });
  }

}
