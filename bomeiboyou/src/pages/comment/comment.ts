import { Component, OnInit } from '@angular/core';
import { NavController, NavParams, ToastController } from 'ionic-angular';
import { FormBuilder, Validators } from '@angular/forms';
import { FormPage } from '../shared/form';
import { DetailPage } from '../detail/detail';
import { Comment } from '../../entity/comment';
import { CartItem } from '../../entity/cart-item';
import { ReviewService } from '../../providers/review';
import { OrderService } from '../../providers/order';

/*
  Generated class for the AddComment page.

  See http://ionicframework.com/docs/v2/components/#navigation for more info on
  Ionic pages and navigation.
*/
@Component({
  selector: 'page-comment',
  templateUrl: 'comment.html'
})
export class CommentPage extends FormPage implements OnInit {
  stars: string[];
  item: CartItem;
  items: CartItem[];
  comment: Comment;
  constructor(public navCtrl: NavController, public params: NavParams, public fb: FormBuilder, public reviewService: ReviewService,
    public orderService: OrderService, public toastCtrl: ToastController) {
    super(navCtrl, true);
    this.item = this.params.data.item;
    this.items = this.params.data.items;

    this.formErrors = {
      'content': []
    };
    this.validationMessages = {
      'content': {
        'required': '请输入商品评价'
      }
    };
  }

  ngOnInit(): void {
    this.detailPage = DetailPage;
    if (!this.item.commentId) {
      this.comment = new Comment("", "", 0, new Date());
      this.stars = Array(5).fill("star-outline");
    }
    //ToDo:编辑评论
    else {

    }

    this.buildForm();
  }

  onSubmit(): void {
    if (!this.isProcessing) {
      this.isProcessing = true;
      this.comment.content = this.form.value.content;
      this.reviewService.addComment(this.item.productId, this.item.orderInfoId, this.comment)
        .then(() => {
          var index = this.items.indexOf(this.item);
          this.items.splice(index, 1);
          this.orderService.getOrderInfos(false);
          let toast = this.toastCtrl.create({
            message: '添加评论成功',
            duration: 600,
            position: 'middle',
            cssClass: "toast-ok"
          });
          toast.present().then(() => {
            this.navCtrl.pop();
          });
        });
    }
  }

  buildForm(): void {
    this.form = this.fb.group({
      'content': [this.comment.content, [
        Validators.required
      ]]
    });

    this.form.valueChanges
      .subscribe(data => this.onValueChanged(data));
  }

  selectStar(index: number): void {
    this.comment.star = index + 1;
    for (let i = 0; i < 5; i++) {
      if (i <= index) {
        this.stars[i] = "star";
      }
      else {
        this.stars[i] = "star-outline";
      }
    }
  }
}
