namespace DutyFree.Admin.Api
{
    using WebCore.Base;
    using Entity.Order;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Platform.Util;
    using Service.Order;
    using System;
    using System.Collections.Generic;

    [Authorize]
    [Route("api/[controller]")]
    public class OrderInfoController : BaseController
    {
        private static ILogger _logger = LoggerUtil.CreateLogger<OrderInfoController>();

        private OrderService _orderService;

        public OrderInfoController(OrderService orderService)
        {
            this._orderService = orderService;
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<OrderInfo> Get()
        {
            try
            {
                return _orderService.GetOrderInfos();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public OrderInfo Get(string id)
        {
            try
            {
                return _orderService.GetOrderInfo(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]OrderInfo value)
        {
            this._orderService.UpdateOrderInfo(value);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(string orderInfoId)
        {
            this._orderService.DeleteOrderInfo(orderInfoId);
        }
    }
}
