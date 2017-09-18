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
    public class OrderController : BaseController
    {
        private static ILogger _logger = LoggerUtil.CreateLogger<OrderController>();

        private OrderService _orderService;

        public OrderController(OrderService orderService)
        {
            this._orderService = orderService;
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<Order> Get()
        {
            try
            {
                return _orderService.Get();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
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
        public void Delete(string orderInfoId)
        {
        }
    }
}
