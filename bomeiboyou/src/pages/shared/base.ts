///<reference path="../../../node_modules/reflect-metadata/reflect-metadata.d.ts"/>"
import { NavController } from 'ionic-angular';
import { Context } from '../../shared/context';

export abstract class BasePage {
    static _headerHeight: number = 0;
    static _footerHeight: number = 0;
    detailPage: any;
    headerHeight: number;
    footerHeight: number;
    isReady: boolean = false;
    pageSelector: string;
    public constructor(public navCtrl: NavController, public isHideBottom = false) {
        this.headerHeight = BasePage._headerHeight;
        this.footerHeight = BasePage._footerHeight;

        let annotations = Reflect.getMetadata('annotations', this.constructor);
        if (annotations instanceof Array && annotations.length > 0) {
            this.pageSelector = annotations[0].selector;
        }
    }

    public ionViewWillEnter(): void {
        if (this.isHideBottom) {
            this.hideBottom();
        }
    }

    public ionViewDidEnter(): void {
        if (this.pageSelector) {
            let elem = <HTMLElement>document.querySelector(this.pageSelector + " .scroll-content");
            if (elem !== null && this.footerHeight > 0) {
                elem.style.marginBottom = this.isHideBottom ? "0" : this.footerHeight + "px";
            }
        }
    }

    public ionViewWillLeave(): void {
        if (this.isHideBottom) {
            //ionViewWilLeave在下一个Page的ionViewWillEnter之前执行，导致footer显示不正确
            if (this.navCtrl.length() > 1) {
                var next = this.navCtrl.last();

                // 当前页面为最后一个页面即为pop, 反之为push
                if (next.instance === this) {
                    next = this.navCtrl.getByIndex(this.navCtrl.length() - 2);
                }

                var nextPage = next.instance as BasePage;

                if (!nextPage.isHideBottom) {
                    this.showBottom();
                }
            }
            else {
                this.showBottom();
            }
        }
    }

    public getImgUrl(imgId: string, height = 0, width = 0): string {
        return Context.HOST_STATIC + imgId + ".jpg";
        // if (height > 0 && width > 0) {
        //     return Context.HOST + "/api/img/" + logo + "?width=" + width + "&height=" + height;
        // }
        // else {
        //     return Context.HOST + "/api/img/" + logo;
        // }
    }

    public hideBottom(): void {
        let elem = <HTMLElement>document.querySelector("div.tabbar");
        if (elem !== null) {
            //elem.classList.remove("show-tabbar");
            elem.style.display = "none";
        }
    }

    public showBottom(): void {
        let elem = <HTMLElement>document.querySelector("div.tabbar");
        if (elem !== null) {
            //elem.classList.add("show-tabbar");
            elem.style.display = "flex";
        }
    }

    public getDefaultImgUrl(): string {
        return "assets/images/blank.gif";
    }

    public take(items: Array<any>, index: number): Array<any> {
        if (items && items.length > 0) {
            return items.filter((val, i) => {
                return i < index;
            });
        }

        return [];
    }

    public getStarNames(star: number): string[] {
        let stars = Array(5).fill("star");
        for (let i = star; i < 5; i++) {
            stars[i] = "star-outline";
        }
        return stars;
    }

    public ceil(number: number): number {
        return Math.ceil(number);
    }

    public goPage(page: BasePage, data?: any): void {
        if (data) {
            this.navCtrl.push(page, data);
        }
        else {
            this.navCtrl.push(page);
        }
    }

    public goDetailPage(productId: string): void {
        this.navCtrl.push(this.detailPage, { productId: productId });
    }
}