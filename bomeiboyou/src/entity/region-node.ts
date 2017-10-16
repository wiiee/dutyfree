import { Region } from './region';

export class RegionNode {
    public constructor(public region: Region, public regions: Region[]) { }
}