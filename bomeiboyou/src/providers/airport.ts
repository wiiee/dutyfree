import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import 'rxjs/add/operator/map';
import { Context } from '../shared/context';
import { BaseService } from './base';
import { Airport } from '../entity/airport';

/*
  Generated class for the Airport provider.

  See https://angular.io/docs/ts/latest/guide/dependency-injection.html
  for more info on providers and Angular 2 DI.
*/
@Injectable()
export class AirportService extends BaseService {

  constructor(public http: Http) {
    super();
  }

  public getAirports(): Promise<Airport[]> {
    return new Promise(resolve => {
        var url = Context.HOST + "/api/airport/GetAirports";
        this.http.get(url).map(res => res.json()).subscribe(data => {
          resolve(this.mapAirports(data));
        });
    });
  }

  public getAirport(airportId: string): Promise<Airport[]> {
    return new Promise(resolve => {
        var url = Context.HOST + "/api/airport/" + airportId;
        this.http.get(url).map(res => res.json()).subscribe(data => {
          resolve(new Airport(data.Id, data.Name, data.Latitude, data.Longitude));
        });
    });
  }  

  private mapAirports(data: Array<any>): Airport[] {
    let result: Airport[] = [];
    data.forEach(o => {
      result.push(new Airport(o.Id, o.Name, o.Latitude, o.Longitude));
    });
    return result;
  }  
}
