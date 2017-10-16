import { NavController, Events } from 'ionic-angular';
import { BasePage } from './base';
import { CartService } from '../../providers/cart';

export abstract class AuthPage extends BasePage {
    titleIndex: number;
    titles: string[];
    public constructor(public navCtrl: NavController, public events: Events, public cartService: CartService, public isHideBottom = false) {
        super(navCtrl, isHideBottom);
        this.titleIndex = 0;
        this.titles = ["登陆", "注册"];
        this.listenToEvents();
    }

    public selectTitle(i: number): void {
        this.titleIndex = i;
    }

    public listenToEvents(): void {
        this.events.subscribe('user:login', () => {
            this.isReady = false;
            this.cartService.reloadCarts().then(() => {
                this.isReady = true;
            });
        });

        this.events.subscribe('user:logoff', () => {
            this.cartService.reloadCarts().then(() => {
                if (this.navCtrl.length() > 1) {
                    this.navCtrl.popToRoot()
                }
            });
        });
    }
}