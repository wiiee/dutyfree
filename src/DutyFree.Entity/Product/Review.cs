namespace DutyFree.Entity.Product
{
    using Platform.Extension;
    using System;
    using System.Collections.Generic;
    using ValueType;

    public class Review : BaseEntity
    {
        //Id为ProductId

        //key为时间点(由于javascript的long精度的问题，使用string类型), List<Comment>为一组评论，包括评论，回复
        public Dictionary<string, List<Comment>> Comments { get; set; }

        public Review(string productId)
        {
            this.Id = productId;
            this.Comments = new Dictionary<string, List<Comment>>();
        }

        public Review()
        {

        }

        public void AddComment(Comment comment)
        {
            if(comment == null)
            {
                return;
            }

            if (comment.Created.IsEmpty())
            {
                comment.Created = DateTime.Now;
            }

            var tick = comment.Created.Ticks.ToString();

            if (Comments.ContainsKey(tick))
            {
                throw new Exception("Duplicated time");
            }

            Comments.Add(tick, new List<Comment>{ comment });
        }
    }
}
