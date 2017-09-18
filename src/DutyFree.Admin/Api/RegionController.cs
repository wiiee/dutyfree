namespace DutyFree.Admin.Api
{
    using WebCore.Base;
    using Entity.Standard;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Platform.Model.GTreeTable;
    using Service.Extension;
    using Service.Standard;
    using System.Collections.Generic;

    [Authorize]
    [Route("api/[controller]")]
    public class RegionController : BaseController
    {
        private RegionService _regionService;

        public RegionController(RegionService regionService)
        {
            this._regionService = regionService;
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public JsonResult Get(string id)
        {
            var nodes = new List<GTreeTable>();
            var regions = new List<Region>();

            if(id == "0")
            {
                regions = _regionService.Get(o => string.IsNullOrEmpty(o.ParentId));
            }
            else
            {
                regions = _regionService.Get(o => o.ParentId == id);
            }

            foreach(var item in regions)
            {
                nodes.Add(new GTreeTable(item.Id, item.Name.GetLocal(), item.GetLevel(), new GTreeData(item.HasChild())));
            }

            object json = new
            {
                nodes = nodes
            };

            return Json(json);
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
