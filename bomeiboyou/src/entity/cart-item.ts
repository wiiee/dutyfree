export class CartItem {
    public constructor(public productId: string, public quantity: number, public isSelected: boolean,
        public logo: string, public name: string, public marketPrice: number, public price: number,
        public commentId: string, public orderInfoId: string, public referencePrice: number, public minPrice: number,
        public maxPrice: number) { }
}