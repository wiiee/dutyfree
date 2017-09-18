namespace DutyFree.Mobile.Api
{
    using Microsoft.AspNetCore.Mvc;
    using Platform.Extension;
    using Platform.Util;
    using System.Collections.Generic;
    using WebCore.Base;

    [Route("api/[controller]")]
    public class GeoController : BaseController
    {
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

        [Route("GetAddress")]
        [HttpGet]
        public string GetAddress(double latitude, double longitude)
        {
            var geo = BaiduMapUtil.GetGeo(latitude, longitude);

            if(geo != null && geo.result != null)
            {
                return string.Format("{0}({1})", geo.result.formatted_address, geo.result.sematic_description);
            }

            return "";
        }

        [Route("GeoConvert")]
        [HttpGet]
        public string GeoConvert(double latitude, double longitude)
        {
            var geo = BaiduMapUtil.GeoConvert(latitude, longitude);
            if (geo != null && geo.result.IsEmpty())
            {
                return string.Format("{0},{1}", geo.result[0].x, geo.result[0].y);
            }
            return string.Empty;
        }

        [Route("GetNearestAirport")]
        [HttpGet]
        public string GetNearestAirport(double latitude, double longitude)
        {
            return string.Empty;
        }
    }
}
