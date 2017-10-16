import { Component } from '@angular/core';
import { Platform, AlertController} from 'ionic-angular';
import { StatusBar } from 'ionic-native';
import { TabsPage } from '../pages/tabs/tabs';
import { InitService } from '../providers/init';

@Component({
  template: `<ion-nav [root]="rootPage"></ion-nav>`
})
export class MyApp {
  // rootPage = TabsPage;
  rootPage: any;
  backPressed: boolean;
  isProcessing: boolean;
  constructor(public platform: Platform, public initService: InitService, public alertCtrl: AlertController) {
    this.backPressed = false;
    this.isProcessing = false;
    platform.ready().then(() => {
      // Okay, so the platform is ready and our plugins are available.
      // Here you can do any higher level native things you might need.
      this.initService.init().then(() => {
        this.rootPage = TabsPage;
        StatusBar.styleDefault();
      });

      if (this.platform.is('android')) {
        platform.backButton.subscribe(() => {
          this.registerBackButtonListener();
        });
      }
    });
  }

  registerBackButtonListener(): void {
    if (this.backPressed && !this.isProcessing) {
      this.confirmExitApp();
    }
    else{
      this.backPressed = true;
      setTimeout(() => {
        this.backPressed = false;
      }, 2000);
    }
  }

  confirmExitApp(): void {
    this.isProcessing = true;
    let confirm = this.alertCtrl.create({
      title: '退出',
      message: '确定退出?',
      buttons: [
        {
          text: '取消',
          handler: () => {
            this.isProcessing = false;
          }
        },
        {
          text: '退出',
          handler: () => {
            this.platform.exitApp();
          }
        }
      ]
    });
    confirm.present();
  }
}
