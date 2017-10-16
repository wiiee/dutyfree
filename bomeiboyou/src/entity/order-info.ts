import { CartItem } from './cart-item';

export class OrderInfo {
    //type: 0为Order, 1为PartnerOrder
    public constructor(public id: string, public items: CartItem[], public totalPrice: number, public status: number, public flightNo: string, public airportId: string, public type: number) {

    }

    public getCount(): number {
        if (this.items.length === 0) {
            return 0;
        }

        return this.items.map(o => o.quantity).reduce((pre, cur) => pre + cur);
    }

    public getPendingReviewCount(): number {
        if (this.items.length === 0 || this.status !== 3) {
            return 0;
        }

        return this.items.map(o => o.commentId ? 0 : 1).reduce((pre, cur) => pre + cur);
    }

    public getDoneReviewCount(): number {
        if (this.items.length === 0 || this.status !== 3) {
            return 0;
        }

        return this.items.map(o => o.commentId ? 1 : 0).reduce((pre, cur) => pre + cur);
    }

    public getReferencePrice() {
        if (this.items.length === 0) {
            return 0;
        }

        return this.items.map(o => o.referencePrice).reduce((pre, cur) => pre + cur);
    }

    public getPrice() {
        if (this.items.length === 0) {
            return 0;
        }

        return this.items.map(o => o.price).reduce((pre, cur) => pre + cur);
    }
}