import { WalletItem } from './wallet-item';

export class Wallet {
    public constructor(public inItems: WalletItem[], public outItems: WalletItem[]) { }

    public getTotal(): number {
        let inTotal = 0;
        let outTotal = 0;

        if (this.inItems.length > 0) {
            inTotal = this.inItems.map(o => o.number).reduce((pre, cur) => pre + cur);
        }

        if (this.outItems.length > 0) {
            outTotal = this.outItems.map(o => o.number).reduce((pre, cur) => pre + cur);
        }

        return inTotal - outTotal;
    }
}