namespace DutyFree.Mobile.Api
{
    using Entity.Product;
    using Entity.ValueType;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Platform.Extension;
    using Platform.Util;
    using Service.Product;
    using System;
    using System.Collections.Generic;
    using WebCore.Base;
    using WebCore.Extension;

    [Route("api/[controller]")]
    public class ReviewController : BaseController
    {
        private ReviewService _reviewService;

        public ReviewController(ReviewService reviewService)
        {
            this._reviewService = reviewService;
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public Review Get(string id)
        {
            var review = this._reviewService.Get(id);
            if(review != null && !review.Comments.IsEmpty())
            {
                foreach(var item in review.Comments)
                {
                    item.Value.ForEach(o => o.UserId = StringUtil.GetSecretPhone(o.UserId));
                }
            }
            return review == null ? new Review() : review;
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [Authorize]
        [Route("AddComment")]
        [HttpPost]
        public void AddComment(string userId, string productId, string orderInfoId, [FromBody]Comment comment)
        {
            if (string.IsNullOrEmpty(userId))
            {
                userId = this.GetUserId();
            }

            comment.UserId = userId;
            comment.Created = DateTime.Now;
            this._reviewService.AddComment(productId, orderInfoId, comment);
        }

        [Route("GetComment")]
        [HttpGet]
        public List<Comment> GetComment(string productId, string commentId)
        {
            var comments = this._reviewService.GetComment(productId, commentId);
            if (!comments.IsEmpty())
            {
                comments.ForEach(o => o.UserId = StringUtil.GetSecretPhone(o.UserId));
            }
            return comments;
        }

        [Route("GetComments")]
        [HttpPost]
        public List<List<Comment>> GetComments([FromBody]List<KeyValuePair<string, string>> commentIds)
        {
            var result = new List<List<Comment>>();

            foreach(var item in commentIds)
            {
                result.Add(this.GetComment(item.Key, item.Value));
            }

            return result;
        }
    }
}
