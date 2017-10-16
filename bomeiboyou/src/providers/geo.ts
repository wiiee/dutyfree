import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import { Geolocation, Geoposition } from 'ionic-native';
import 'rxjs/add/operator/map';
import { BaseService } from './base';
import { Location } from '../entity/location';
import { Context } from '../shared/context';

/*
  Generated class for the Geo provider.

  See https://angular.io/docs/ts/latest/guide/dependency-injection.html
  for more info on providers and Angular 2 DI.
*/
@Injectable()
export class GeoService extends BaseService {
  location: Location;
  constructor(public http: Http) {
    super();
  }

  public getLocation(): Promise<Location> {
    if (this.location) {
      return Promise.resolve(this.location);
    }

    this.location = new Location(0, 0, "");
    var options = { maximumAge: 300000, timeout: 60000, enableHighAccuracy: true };
    Geolocation.watchPosition(options).subscribe(position => {
      if ((<Geoposition>position).coords) {
        this.updateLocation(<Geoposition>position);
      }
    });

    return new Promise(resolve => {
      Geolocation.getCurrentPosition().then((resp) => {
        if (resp) {
          this.updateLocation(resp, true).then(() => {
            resolve(this.location);
          });
        }
        else {
          resolve(this.location);
        }
      }).catch((error) => {
        resolve(this.location);
      });
    });
  }

  public getNearestAirport(): Promise<string> {
    return new Promise(resolve => {
      if (this.location) {
        var url = Context.HOST + "/api/geo/getNearestAirport?latitude=" + this.location.latitude + "&longitude=" + this.location.longitude;
        this.http.get(url).map(res => res.text()).subscribe(data => {
          resolve(data);
        });
      }
      else {
        resolve("");
      }
    });
  }

  private updateLocation(postion: Geoposition, isGetAddress: boolean = false): Promise<void> {
    return new Promise(resolve => {
      this.location.latitude = postion.coords.latitude;
      this.location.longitude = postion.coords.longitude;
      if (isGetAddress) {
        this.getAddress(this.location.latitude, this.location.longitude).then(address => {
          this.location.address = address;
          resolve();
        });
      }
    });
  }

  // private onSuccess(position): void {
  //   this.location.latitude = position.coords.latitude;
  //   this.location.longitude = position.coords.longitude;
  // }

  // private onError(error): void {
  //   console.log('code: ' + error.code + '\n' + 'message: ' + error.message + '\n');
  // }

  public getAddress(latitude: number, longitude: number): Promise<string> {
    return new Promise(resolve => {
      var url = Context.HOST + "/api/geo/GetAddress?latitude=" + latitude + "&longitude=" + longitude;
      this.http.get(url).map(res => res.text()).subscribe(data => {
        resolve(data);
      });
    });
  }
}
