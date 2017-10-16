import { Component, OnInit, ViewChild } from '@angular/core';
import { NavController, ToastController, Content, AlertController } from 'ionic-angular';
import { BasePage } from '../shared/base';
import { DetailPage } from '../detail/detail';
import { CartItem } from '../../entity/cart-item';
import { Airport } from '../../entity/airport';
import { Flight } from '../../entity/flight';
import { Page } from '../../entity/page';
import { Constant } from '../../shared/constant';
import { Location } from '../../entity/location';
import { PartnerOrderService } from '../../providers/partner-order';
import { CartService } from '../../providers/cart';
import { GeoService } from '../../providers/geo';
import { FlightService } from '../../providers/flight';
import { AirportService } from '../../providers/airport';

/*
  Generated class for the HomePartner page.

  See http://ionicframework.com/docs/v2/components/#navigation for more info on
  Ionic pages and navigation.
*/
@Component({
  selector: 'page-home-partner',
  templateUrl: 'home-partner.html'
})
export class HomePartnerPage extends BasePage implements OnInit {
  @ViewChild('content') content: Content;
  items: CartItem[];
  flights: Flight[];
  flightNo: string;
  page: Page;
  location: Location;
  airportId: string;
  airports: Airport[];
  error: string;
  time: Date;
  constructor(public navCtrl: NavController, public toastCtrl: ToastController, public partnerOrderService: PartnerOrderService, public cartService: CartService,
    public geoService: GeoService, public flightService: FlightService, public airportService: AirportService, public alertCtrl: AlertController) {
    super(navCtrl);
    this.items = [];
    this.time = new Date();
  }

  ionViewWillEnter(): void {
    super.ionViewWillEnter();
    this.partnerOrderService.getItems(this.page).then(data => {
      this.items = data;
    });
  }

  ionViewDidEnter(): void {
    if (BasePage._footerHeight === 0) {
      BasePage._headerHeight = this.content.contentTop;
      BasePage._footerHeight = this.content.contentBottom;
    }
  }

  ngOnInit(): void {
    setInterval(() => {
      this.time = new Date();
    }, 1000);

    this.detailPage = DetailPage;
    this.page = new Page(0, Constant.PAGE_SIZE);

    this.geoService.getLocation().then(data => this.location = data);

    // this.geoService.getNearestAirport().then(data => {
    //   if(data){
    //     this.airportId = data;
    //   }
    //   else{
    //     this.error = "没有检测到你在机场附近，暂时不开放购买。";
    //   }
    // });

    this.cartService.getActivePartnerOrderInfo().then(data => {
      if (data) {
        this.airportId = data.airportId;
        this.flightNo = data.flightNo;

        if (this.airportId) {
          this.flightService.getFlights(this.airportId).then(flights => {
            this.flights = flights;
            if(this.flightNo && this.flights.findIndex(o => o.id === this.flightNo) === -1){
              this.flightNo = null;
              this.partnerOrderService.setFlightNo(this.flightNo);
            }
          });
        }
      }
    });

    this.airportService.getAirports().then(data => {
      this.airports = data;
    });
  }

  addCart(item: CartItem): void {
    if (this.flightNo) {
      this.addProduct(item);
    }
    //还没有选择航班
    else {
      if (this.airportId) {
        this.selectFlight(item);
      }
      else {
        this.selectAirport();
      }
    }
  }

  private addProduct(item: CartItem): void {
    this.cartService.addPartnerCart(item);
    this.partnerOrderService.getItems(this.page).then(data => {
      this.items = data;
    });
    this.showToast();
  }

  selectFlight(item?: CartItem): void {
    if (this.flights && this.flights.length > 0) {
      let alert = this.alertCtrl.create();
      alert.setTitle('请选择您的航班');

      this.flights.forEach(o => {
        alert.addInput({
          type: 'radio',
          label: o.id,
          value: o.id,
          checked: o.id === this.flightNo
        });
      });

      alert.addButton('取消');
      alert.addButton({
        text: '确定',
        handler: data => {
          if (data && this.flightNo !== data) {
            this.flightNo = data;
            this.partnerOrderService.setFlightNo(this.flightNo);
            this.flightService.isFlightValid(this.flightNo).then(data => {
              if (data.isSuccessful) {
                this.error = null;
                if (item) {
                  this.addProduct(item);
                }
              }
              else {
                this.error = data.msg;
              }
            })
          }
        }
      });
      alert.present();
    }
  }

  selectAirport(): void {
    let alert = this.alertCtrl.create();
    alert.setTitle('请选择您的机场');

    this.airports.forEach(o => {
      alert.addInput({
        type: 'radio',
        label: o.name,
        value: o.id,
        checked: o.id === this.airportId
      });
    });

    alert.addButton('取消');
    alert.addButton({
      text: '确定',
      handler: data => {
        if (data && data !== this.airportId) {
          this.airportId = data;
          this.flightNo = null;
          this.partnerOrderService.setFlightNo(this.flightNo);
          this.partnerOrderService.setAirportId(this.airportId);
          this.flightService.getFlights(this.airportId).then(data => {
            this.flights = data;
          });
        }
      }
    });
    alert.present();
  }

  getDepartureTime(): string {
    if (this.flightNo && this.flights && this.flights.length > 0 && this.flights.findIndex(o => o.id === this.flightNo) !== -1) {
      return this.flights.find(o => o.id === this.flightNo).departureTime.getTime();
    }

    return "";
  }

  getAirportName(): string {
    if (this.airportId && this.airports) {
      return this.airports.find(o => o.id === this.airportId).name;
    }

    return "";
  }

  showToast(): void {
    let toast = this.toastCtrl.create({
      message: '加入购物车成功',
      duration: 600,
      position: 'middle',
      cssClass: "toast-ok"
    });
    toast.present();
  }

  showEarningRule(item: CartItem): void {

  }
}