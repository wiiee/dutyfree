import { NgModule } from '@angular/core';
import { IonicApp, IonicModule } from 'ionic-angular';
import { MyApp } from './app.component';
import { InformationPage } from '../pages/information/information';
import { CartPage } from '../pages/cart/cart';
import { HomePage } from '../pages/home/home';
import { TabsPage } from '../pages/tabs/tabs';
import { ConfirmOrderPage } from '../pages/confirm-order/confirm-order';
import { AddressPage } from '../pages/address/address';
import { AddressListPage } from '../pages/address-list/address-list';
import { BrandPage } from '../pages/brand/brand';
import { DetailPage } from '../pages/detail/detail';
import { PaymentPage } from '../pages/payment/payment';
import { ProductListPage } from '../pages/product-list/product-list';
import { RegionPage } from '../pages/region/region';
import { OrderListPage } from '../pages/order-list/order-list';
import { WalletPage } from '../pages/wallet/wallet';
import { MessagePage } from '../pages/message/message';
import { CommentPage } from '../pages/comment/comment';
import { QrPage } from '../pages/qr/qr';
import { CommentListPage } from '../pages/comment-list/comment-list';
import { HomePartnerPage } from '../pages/home-partner/home-partner';
import { CartPartnerPage } from '../pages/cart-partner/cart-partner';
import { SettingPage } from '../pages/setting/setting';
import { AccountPage } from '../pages/account/account';
import { LogInComponent } from '../components/log-in/log-in';
import { SignUpComponent } from '../components/sign-up/sign-up';
import { Context } from '../shared/context';
import { InitService } from '../providers/init';
import { PreferenceService } from '../providers/preference';
import { ProductService } from '../providers/product';
import { UserService } from '../providers/user';
import { RegionService } from '../providers/region';
import { BrandService } from '../providers/brand';
import { OrderService } from '../providers/order';
import { ReviewService } from '../providers/review';
import { PartnerOrderService } from '../providers/partner-order';
import { CartService } from '../providers/cart';
import { GeoService } from '../providers/geo';
import { CameraService } from '../providers/camera';
import { TransferService } from '../providers/transfer';
import { FlightService } from '../providers/flight';
import { AirportService } from '../providers/airport';

@NgModule({
  declarations: [
    MyApp,
    InformationPage,
    CartPage,
    HomePage,
    TabsPage,
    ConfirmOrderPage,
    AddressPage,
    AddressListPage,
    BrandPage,
    DetailPage,
    PaymentPage,
    ProductListPage,
    RegionPage,
    OrderListPage,
    WalletPage,
    MessagePage,
    CommentPage,
    QrPage,
    CommentListPage,
    HomePartnerPage,
    CartPartnerPage,
    SettingPage,
    AccountPage,
    LogInComponent,
    SignUpComponent
  ],
  imports: [
    IonicModule.forRoot(MyApp)
  ],
  bootstrap: [IonicApp],
  entryComponents: [
    MyApp,
    InformationPage,
    CartPage,
    HomePage,
    TabsPage,
    ConfirmOrderPage,
    AddressPage,
    AddressListPage,
    BrandPage,
    DetailPage,
    PaymentPage,
    ProductListPage,
    RegionPage,
    OrderListPage,
    WalletPage,
    MessagePage,
    CommentPage,
    QrPage,
    CommentListPage,
    HomePartnerPage,
    CartPartnerPage,
    SettingPage,
    AccountPage,
    LogInComponent,
    SignUpComponent
  ],
  providers: [Context, InitService, PreferenceService, ProductService, UserService, RegionService, BrandService, AirportService, 
  OrderService, ReviewService, PartnerOrderService, CartService, GeoService, CameraService, TransferService, FlightService]
})
export class AppModule {}
