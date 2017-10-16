import { Region } from './region';

export class Address {
    public constructor(public name: string, public phone: string, public text: string, public regions: Region[], public isDefault: boolean){}

    public isEmpty(): boolean {
        if(this.name){
            return false;
        }

        if(this.phone){
            return false;
        }

        if(this.text){
            return false;
        }

        if(this.regions && this.regions.length > 0){
            return false;
        }

        return true;
    }

    public getRegionText(): string {
        if(this.regions && this.regions.length > 0){
            return this.regions.map(o => o.name).reduce((pre, cur) => pre + cur);
        }

        return "";
    }
}