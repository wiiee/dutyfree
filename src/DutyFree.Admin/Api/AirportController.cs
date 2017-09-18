namespace DutyFree.Admin.Api
{
    using WebCore.Base;
    using Entity.Standard;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Platform.Model.GTreeTable;
    using Service.Standard;
    using System.Collections.Generic;
    using System.Linq;

    [Authorize]
    [Route("api/[controller]")]
    public class AirportController : BaseController
    {
        private AirportService _airportService;

        public AirportController(AirportService airportService)
        {
            this._airportService = airportService;
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
            var airports = new List<Airport>();
            var countryIds = _airportService.Get().Select(o => o.CountryId).Distinct().OrderBy(o => o).ToList();

            if (id == "0")
            {
                foreach (var item in countryIds)
                {
                    nodes.Add(new GTreeTable(item, item, 0, new GTreeData(true)));
                }

                object json = new
                {
                    nodes = nodes
                };

                return Json(json);
            }
            else if (countryIds.Contains(id))
            {
                airports = _airportService.Get(o => o.CountryId == id);

                foreach (var item in airports)
                {
                    nodes.Add(new GTreeTable(item.Id, item.Name.GetLocal(), 1, new GTreeData(false)));
                }

                object json = new
                {
                    nodes = nodes
                };

                return Json(json);
            }
            else
            {
                object json = new
                {
                    nodes = airports
                };

                return Json(json);
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
