import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import 'rxjs/add/operator/map';
import { BaseService } from './base';
import { Transfer } from 'ionic-native';

/*
  Generated class for the TransferService provider.

  See https://angular.io/docs/ts/latest/guide/dependency-injection.html
  for more info on providers and Angular 2 DI.
*/
@Injectable()
export class TransferService extends BaseService {
    constructor(public http: Http) {
        super();
    }

    public upload(fileUrl: string, url: string): Promise<void> {
        return new Promise(resolve => {
            const fileTransfer = new Transfer();
            var options: any;

            var name = fileUrl.substr(fileUrl.lastIndexOf('/') + 1);

            options = {
                fileKey: 'file',
                fileName: name
            };

            fileTransfer.upload(fileUrl, url, options)
                .then((data) => {
                    // success
                    resolve();
                }, (err) => {
                    // error
                    resolve();
                });
        });
    }
}