import { Component, OnInit, ViewChild } from '@angular/core';
import { NavController, LoadingController, Content } from 'ionic-angular';
import { Brand } from '../../entity/brand';
import { BrandService } from '../../providers/brand';
import { BrandPage } from '../brand/brand';
import { BasePage } from '../shared/base';

@Component({
  selector: 'page-home',
  templateUrl: 'home.html'
})
export class HomePage extends BasePage implements OnInit {
  @ViewChild('content') content: Content;
  public current: string = "";
  public brands: Brand[];
  public groups: Array<Array<Brand>>;
  // public names: any;
  public items: Array<[string, string]>;

  public constructor(public navCtrl: NavController, public loadingCtrl: LoadingController, public brandService: BrandService) {
    super(navCtrl);
  }

  ngOnInit(): void {
    // this.names = {
    //   "AVÈNE": "AVENE",
    //   "ESTĒE LAUDER": "ESTEE LAUDER",
    //   "LANCÔME": "LANCOME",
    //   "PUUJÄPUU": "PUUJAPUU",
    //   "同仁堂": "TRT",
    //   "张君雅": "ZJY",
    //   "我的美丽日记": "WDMLRJ",
    //   "森田药妆": "STYZ",
    //   "美好人生": "MHRS",
    //   "贵爱娘": "GAN",
    //   "COW & GATE": "COWGATE",
    //   "DOLCE & GABBANA": "DOLCEGABBANA",
    //   "LOCK&LOCK": "LOCKLOCK",
    //   "TESORI D’ORIENTE": "TESORI DORIENTE",
    //   "IT'S SKIN": "ITS SKIN",
    //   "LUCAS' PAPAW REMEDIES": "LUCAS PAPAW REMEDIES",
    //   "LΛNEIGE": "LANEIGE",
    //   "MILO&GABBY": "MILOGABBY"
    // };

    this.items = [
      ["", "国际品牌"],
      ["Cosmetics", "个护化妆"],
      ["Perfume", "香水"],
      ["Bag", "品牌箱包"],
      ["Health", "营养保健"],
      ["Cloth", "名流服饰"],
      ["Electric", "家用电器"],
      ["Digit", "手机数码"],
      ["Baby", "母婴童装"]
    ]

    this.brands = [];
    this.groups = [];

    this.brandService.getBrands("")
      .then(data => {
        this.brands = data;
        this.updateGroups();
      });
  }

  updateGroups(): void {
    this.groups = [];
    for (let i = 0; i < Math.ceil(this.brands.length / 3); i++) {
      let group = [];
      let start = i * 3;
      for (let j = 0; j < 3; j++) {
        if (start + j < this.brands.length) {
          group.push(this.brands[start + j]);
        }
      }
      this.groups.push(group);
    }
  }

  searchItems(e): void {

  }

  itemSelected(item: string): void {
    this.current = item[0];
    //this.presentLoading();

    this.brandService.getBrands(item[0])
      .then(data => {
        this.brands = data;
        this.updateGroups();
      });
  }

  // getBrandImgUrl(brand: Brand): string {
  //   var name = brand.id;

  //   if (this.names[name]) {
  //     name = this.names[name];
  //   }

  //   return "assets/images/brand/" + name + ".jpg";
  // }

  goBrand(brand: Brand): void {
    this.navCtrl.push(BrandPage,
      { brand: brand });
  }

  ionViewDidEnter(): void {
    if (BasePage._footerHeight === 0) {
      BasePage._headerHeight = this.content.contentTop;
      BasePage._footerHeight = this.content.contentBottom;
    }
  }

  getMargin(): string {
    return "-" + (BasePage._headerHeight + 5) + "px 0 -" + (BasePage._footerHeight + 5) + "px 0";
  }

  getPadding(): string {
    return BasePage._headerHeight + "px 0 " + BasePage._footerHeight + "px 0";
  }
}