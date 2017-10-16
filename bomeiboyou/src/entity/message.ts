export class Message {
    public constructor(public content: string, public isRead: boolean, public senderId: string, public created: Date) { }
}