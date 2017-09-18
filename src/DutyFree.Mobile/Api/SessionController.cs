namespace DutyFree.Mobile.Api
{
    using WebCore.Base;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using WebCore.Extension;

    [Route("api/[controller]")]
    public class SessionController : BaseController
    {
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(string id)
        {
            return HttpContext.GetSessionValue(id);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {

        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(string id, string value)
        {
            HttpContext.SetSessionValue(id, value);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            HttpContext.RemoveSession(id);
        }
    }
}
