import { Comment } from './comment';

export class Review  {
    public constructor(public productId: string, public comments: Array<Comment[]>) { }
}
