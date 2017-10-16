import { Component, OnInit } from '@angular/core';
import { NavController, NavParams } from 'ionic-angular';
import { BasePage } from '../shared/base';
import { DetailPage } from '../detail/detail';
import { CommentPage } from '../comment/comment';
import { CartItem } from '../../entity/cart-item';
import { Comment } from '../../entity/comment';
import { ReviewService } from '../../providers/review';

/*
  Generated class for the CommentList page.

  See http://ionicframework.com/docs/v2/components/#navigation for more info on
  Ionic pages and navigation.
*/
@Component({
  selector: 'page-comment-list',
  templateUrl: 'comment-list.html'
})
export class CommentListPage extends BasePage implements OnInit {
  isDone: boolean;
  title: string;
  items: CartItem[];
  comments: Array<Comment[]>;
  constructor(public navCtrl: NavController, public params: NavParams, public reviewService: ReviewService) {
    super(navCtrl, true);

    this.isDone = this.params.data.isDone;

    if (this.params.data.items) {
      this.items = this.params.data.items;
    }
  }

  ngOnInit(): void {
    this.detailPage = DetailPage;
    this.title = this.isDone ? "已评价" : "待评价";

    if(!this.isDone){
      this.comments = [];
      this.isReady = true;
    }
    //获取评论
    else{
      this.reviewService.getComments(this.items).then(data => {
        this.comments = data;
        this.isReady = true;
      });
    }
  }

  goCommentPage(item: CartItem): void {
    this.navCtrl.push(CommentPage, {
      item: item,
      items: this.items
    });
  }
}
