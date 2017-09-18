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
    public class PartnerOrderController : BaseController
    {
        private static ILogger _logger = LoggerUtil.CreateLogger<PartnerOrderController>();

        private PartnerOrderService _partnerOrderService;

        public PartnerOrderController(PartnerOrderService partnerOrderService)
        {
            this._partnerOrderService = partnerOrderService;
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<PartnerOrder> Get()
        {
            return this._partnerOrderService.Get();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public PartnerOrder Get(string id)
        {
            try
            {
                return this._partnerOrderService.Get(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
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
    }
}
