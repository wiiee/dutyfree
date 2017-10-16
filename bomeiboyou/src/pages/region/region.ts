import { Component, OnInit } from '@angular/core';
import { NavController, NavParams } from 'ionic-angular';
import { RegionNode } from '../../entity/region-node';
import { Region } from '../../entity/region';
import { Address } from '../../entity/address';
import { BasePage } from '../shared/base';
import { RegionService } from '../../providers/region';

/*
  Generated class for the RegionPage page.

  See http://ionicframework.com/docs/v2/components/#navigation for more info on
  Ionic pages and navigation.
*/
@Component({
  selector: 'page-region',  
  templateUrl: 'region.html'
})
export class RegionPage extends BasePage implements OnInit {
  nodes: RegionNode[];
  currentIndex: number;
  numbers: Array<number[]>;
  groupSize: number;
  title: string;
  address: Address;
  hello = 2;
  constructor(public navCtrl: NavController, public params: NavParams, public regionService: RegionService) {
    super(navCtrl, true);
    this.groupSize = 3;

    this.nodes = [];
    this.numbers = [];

    this.address = this.params.data.address;

    if (this.address.regions && this.address.regions.length > 0) {
      this.nodes = this.address.regions.map(o => new RegionNode(o, []))
      this.title = "编辑地区";
    }
    else {
      this.nodes.push(new RegionNode(null, null));
      this.title = "添加地区";
    }

    this.currentIndex = this.nodes.length - 1;
    this.nodes.forEach(o => this.numbers.push([]));
  }

  ngOnInit(): void {
    this.nodes.forEach((x, i) => {
      this.regionService.getParallelNodes(x.region ? x.region.id : "")
        .then(data => {
          this.nodes[i].regions = data;
          this.numbers[i] = Array(Math.ceil(data.length / this.groupSize)).fill(1).map((x, i) => i);
        }
        );
    });
  }

  public getGroup(groupIndex: number): Region[] {
    let result = [];
    let items = this.nodes[this.currentIndex].regions;

    for (let i = 0; i < this.groupSize; i++) {
      if (groupIndex * this.groupSize + i < items.length) {
        result.push(items[groupIndex * this.groupSize + i]);
      }
    }

    return result;
  }

  public itemSelected(region: Region): void {
    //如果点击的当前有数据
    if (this.nodes[this.currentIndex].region) {
      //如果数据不一样，清空后面的数据
      if (this.nodes[this.currentIndex].region.id !== region.id) {
        this.nodes.splice(this.currentIndex + 1, this.nodes.length - this.currentIndex - 1);
      }
      //如果数据一样，不做任何事情
      else {
        return;
      }
    }

    this.nodes[this.currentIndex].region = region;

    this.regionService.getRegionsByParentId(region.id)
      .then(data => {
        if (data.length === 0) {
          //保存地址，然后退出
          setTimeout(() => this.save(), 1000);
        }
        else {
          this.nodes.push(new RegionNode(null, data));
          this.numbers.push(Array(Math.ceil(data.length / this.groupSize)).fill(1).map((x, i) => i));
          this.currentIndex++;
        }
      });
  }

  public save(): void {
    this.address.regions = this.nodes.map(o => o.region);
    this.navCtrl.pop();
  }

  public isHighlight(id: string): boolean {
    return this.nodes[this.currentIndex].region && this.nodes[this.currentIndex].region.id === id;
  }

  public selectIndex(i: number): void {
    this.currentIndex = i;
  }
}