import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import 'rxjs/add/operator/map';
import { Review } from '../entity/review';
import { Comment } from '../entity/comment';
import { CartItem } from '../entity/cart-item';
import { KeyValuePair } from '../entity/key-value-pair';
import { Context } from '../shared/context';
import { BaseService } from './base';

/*
  Generated class for the ReviewService provider.

  See https://angular.io/docs/ts/latest/guide/dependency-injection.html
  for more info on providers and Angular 2 DI.
*/
@Injectable()
export class ReviewService extends BaseService {
    constructor(public http: Http, public context: Context) {
        super();
    }

    public getReview(productId: string): Promise<Review> {
        return new Promise(resolve => {
            var url = Context.HOST + "/api/review/" + productId;
            this.http.get(url, this.options)
                .map(res => res.json())
                .subscribe(data => {
                    if (data && data.Comments && Object.keys(data.Comments).length > 0) {
                        let result = [];
                        let keys = Object.keys(data.Comments);
                        keys.forEach(o => {
                            let comments = data.Comments[o] as Array<any>;
                            let items = [];
                            comments.forEach(p => {
                                items.push(new Comment(p.UserId, p.Content, p.Star, new Date(p.Created)));
                            });
                            result.push(items);
                        });
                        resolve(new Review(productId, result));
                    }
                    else {
                        resolve(null);
                    }
                });
        });
    }

    public addComment(productId: string, orderInfoId: string, comment: Comment): Promise<void> {
        return new Promise(resolve => {
            var url = Context.HOST + "/api/review/AddComment?productId=" + productId + "&orderInfoId=" + orderInfoId;
            this.http.post(url, JSON.stringify(comment), this.options).subscribe(() => {
                resolve();
            });
        });
    }

    public getComment(productId: string, commentId: string): Promise<Comment[]> {
        return new Promise(resolve => {
            var url = Context.HOST + "/api/review/getComment?productId=" + productId + "&commentId=" + commentId;
            this.http.get(url, this.options).map(res => res.json()).subscribe(data => {
                if (data && data.length > 0) {
                    let result = [];
                    data.forEach(o => {
                        result.push(new Comment(o.UserId, o.Content, o.Star, new Date(o.Created)));
                    });
                    resolve(result);
                }
                else {
                    resolve([]);
                }
            });
        });
    }

    public getComments(items: CartItem[]): Promise<Array<Comment[]>> {
        return new Promise(resolve => {
            var url = Context.HOST + "/api/review/getComments";
            let pairs: KeyValuePair[] = [];
            items.forEach(o => {
                pairs.push(new KeyValuePair(o.productId, o.commentId));
            });

            this.http.post(url, JSON.stringify(pairs), this.options)
                .map(res => res.json()).subscribe(data => {
                    if (data && data.length > 0) {
                        let result = [];
                        data.forEach(o => {
                            let comments = [];
                            o.forEach(p => {
                                comments.push(new Comment(p.UserId, p.Content, p.Star, new Date(p.Created)));
                            });
                            result.push(comments);
                        });
                        resolve(result);
                    }
                    else {
                        resolve([]);
                    }
                });
        });
    }
}

