namespace DutyFree.Service.Product
{
    using Context;
    using Entity.Product;
    using Entity.ValueType;
    using Order;
    using Platform.Context;
    using Platform.Util;
    using System.Collections.Generic;

    public class ReviewService : BaseService<Review>
    {
        public ReviewService(IContextRepository contextRepository) : base(contextRepository) { }

        public List<Comment> GetComment(string productId, string commentId)
        {
            var review = this.Get(productId);
            
            if(review != null && review.Comments.ContainsKey(commentId))
            {
                return review.Comments[commentId];
            }

            return new List<Comment>();
        }

        public void AddComment(string productId, string orderInfoId, Comment comment)
        {
            if(string.IsNullOrEmpty(productId) || string.IsNullOrEmpty(orderInfoId) 
                || ServiceFactory.Instance.GetService<OrderService>().GetOrderInfo(orderInfoId) == null
                || ServiceFactory.Instance.GetService<ProductService>().Get(productId) == null)
            {
                return ;
            }

            var review = this.Get(productId);

            if(review == null)
            {
                review = new Review(productId);
                base.Create(review);
            }

            review.AddComment(comment);
            base.Update(review);

            ServiceFactory.Instance.GetService<OrderService>().UpdateCommentId(orderInfoId, productId, comment.Created.Ticks.ToString());
        }
    }
}
