import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import 'rxjs/add/operator/map';
import { Context } from '../shared/context';
import { BaseService } from './base';
import { Flight } from '../entity/flight';
import { Time } from '../entity/time';
import { ServiceResult } from '../entity/service-result';

/*
  Generated class for the Flight provider.

  See https://angular.io/docs/ts/latest/guide/dependency-injection.html
  for more info on providers and Angular 2 DI.
*/
@Injectable()
export class FlightService extends BaseService {

  constructor(public http: Http) {
    super();
  }

  public getFlights(airportId: string): Promise<Flight[]> {
    return new Promise(resolve => {
        var url = Context.HOST + "/api/flight/GetFlights?airportId=" + airportId;
        this.http.get(url).map(res => res.json()).subscribe(data => {
          resolve(this.mapFlights(data));
        });
    });
  }

  public getFlight(flightNo: string): Promise<Flight> {
    return new Promise(resolve => {
        var url = Context.HOST + "/api/flight/" + flightNo;
        this.http.get(url).map(res => res.json()).subscribe(data => {
          resolve(data);
        });
    });
  }

  public isFlightValid(flightNo: string): Promise<ServiceResult> {
    return new Promise(resolve => {
        var url = Context.HOST + "/api/flight/IsFlightValid?flightNo=" + flightNo;
        this.http.get(url).map(res => res.json()).subscribe(data => {
          resolve(new ServiceResult(data.IsSuccessful, data.Msg));
        });
    });
  }

  private mapFlights(data: Array<any>): Flight[] {
    let result: Flight[] = [];
    data.forEach(o => {
      result.push(new Flight(o.Id, o.DepartureAirport, o.ArrivalAirport, new Time(o.DepartureTime.Hour, o.DepartureTime.Minute, o.DepartureTime.Second), 
      new Time(o.ArrivalTime.Hour, o.ArrivalTime.Minute, o.ArrivalTime.Second)));
    });
    return result;
  }
}
