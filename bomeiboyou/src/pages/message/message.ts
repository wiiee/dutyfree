import { Component, OnInit } from '@angular/core';
import { NavController, NavParams } from 'ionic-angular';
import { BasePage } from '../shared/base';
import { Message } from '../../entity/message';

/*
  Generated class for the Message page.

  See http://ionicframework.com/docs/v2/components/#navigation for more info on
  Ionic pages and navigation.
*/
@Component({
  selector: 'page-message',
  templateUrl: 'message.html'
})
export class MessagePage extends BasePage implements OnInit {
  messages: Message[]
  constructor(public navCtrl: NavController, public params: NavParams) {
    super(navCtrl);
    this.messages = this.params.data.messages;
  }

  ngOnInit(): void {

  }
}
