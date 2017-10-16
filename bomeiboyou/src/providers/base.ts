import { Headers, RequestOptionsArgs } from '@angular/http';
import 'rxjs/add/operator/map';

export abstract class BaseService {
  public options: RequestOptionsArgs;
  constructor() {
    let headers = new Headers();
    headers.append("Content-Type", "application/json; charset=utf-8");
    this.options = {
      headers: headers,
      withCredentials: true
    };
  }
}

