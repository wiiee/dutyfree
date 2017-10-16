export class Product {
    constructor(public id: string, public name: string, public logo: string, public imgIds: string[], public description: string, public descriptionImgIds: string[],
        public marketPrice: number, public price: number, public referencePrice: number, public minPrice: number, public maxPrice: number) {
    }
}