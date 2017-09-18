namespace DutyFree.Mobile.Api
{
    using WebCore.Base;
    using Microsoft.AspNetCore.Mvc;
    using Platform.Extension;
    using Service.Standard;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Authorization;

    [Authorize]
    [Route("api/[controller]")]
    public class RegionController : BaseController
    {
        private static readonly string CHINA = "China";
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

        [Route("GetRegionChilds")]
        [HttpPost]
        public object GetRegionChilds(string regionId)
        {
            var regions = this._regionService.Get(o => o.ParentId == regionId);

            return regions.Select(o => new
            {
                Id = o.Id,
                Name = o.Name.GetLocal()
            }).ToList();
        }

        [Route("GetParallelNodes")]
        [HttpGet]
        public object GetParallelNodes(string regionId)
        {
            if (string.IsNullOrEmpty(regionId))
            {
                return _regionService.Get(o => o.ParentId == CHINA).Select(o => new
                {
                    Id = o.Id,
                    Name = o.Name.GetLocal()
                }).ToList();
            }

            
            var region = _regionService.Get(regionId);

            var regions = _regionService.Get(o => o.ParentId == region.ParentId);

            return regions.Select(o => (object)new
            {
                Id = o.Id,
                Name = o.Name.GetLocal()
            }).ToList();
        }

        [Route("GetNextNodes")]
        [HttpGet]
        public object GetNextNodes(string regionId)
        {
            if (string.IsNullOrEmpty(regionId))
            {
                return _regionService.Get(o => o.ParentId == CHINA).Select(o => (object)new
                {
                    Id = o.Id,
                    Name = o.Name.GetLocal()
                }).ToList();
            }


            var region = _regionService.Get(regionId);

            var regions = _regionService.Get(o => o.ParentId == regionId);

            return regions.Select(o => new
            {
                Id = o.Id,
                Name = o.Name.GetLocal()
            }).ToList();
        }
    }
}
