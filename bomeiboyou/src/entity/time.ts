export class Time {
    public constructor(public hour: number, public minute: number, public second: number) { }

    public getTime(): string {
        return ("0" + this.hour).slice(-2) + ":" + ("0" + this.minute).slice(-2) + ":" + ("0" + this.second).slice(-2);
    }
}