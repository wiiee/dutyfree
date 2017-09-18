namespace DutyFree.Mobile.Api
{
    using Entity.Order;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Platform.Extension;
    using Platform.Util.Domain;
    using Service.Order;
    using System.Collections.Generic;
    using WebCore.Base;
    using WebCore.Extension;

    [Authorize]
    [Route("api/[controller]")]
    public class OrderController : BaseController
    {
        private OrderService _orderService;

        public OrderController(OrderService orderService)
        {
            this._orderService = orderService;
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
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

        [Route("CreateOrderInfo")]
        [HttpGet]
        public string CreateOrderInfo()
        {
            var userId = this.GetUserId();
            return _orderService.CreateOrderInfo(userId);
        }

        [Route("RemoveOrderInfo")]
        [HttpGet]
        public void RemoveOrderInfo(string orderInfoId)
        {
            if(OrderUtil.GetUserId(orderInfoId) == this.GetUserId())
            {
                _orderService.DeleteOrderInfo(orderInfoId);
            }
        }

        [Route("GetOrderInfos")]
        [HttpGet]
        public List<OrderInfo> GetOrderInfos()
        {
            var userId = this.GetUserId();

            var order = _orderService.Get(userId);

            if(order != null && !order.OrderInfos.IsEmpty())
            {
                return order.OrderInfos;
            }

            return new List<OrderInfo>();
        }

        [Route("PayOrderInfo")]
        [HttpGet]
        public void PayOrderInfo(string orderInfoId)
        {
            _orderService.PayOrderInfo(orderInfoId);
        }
    }
}
