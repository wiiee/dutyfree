namespace DutyFree.Mobile.Api
{
    using Entity.Standard;
    using Microsoft.AspNetCore.Mvc;
    using Platform.Extension;
    using Service.Standard;
    using System.Collections.Generic;
    using System.Linq;
    using WebCore.Base;

    [Route("api/[controller]")]
    public class AirportController : BaseController
    {
        private AirportService _airportService;
        private FlightService _flightService;

        public AirportController(AirportService airportSerivce, FlightService flightService)
        {
            this._airportService = airportSerivce;
            this._flightService = flightService;
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

        [Route("GetAirports")]
        [HttpGet]
        public object GetAirports()
        {
            var airports = new List<Airport>();
            string[] airportIds = { "MEL", "SYD", "SEL", "DXB", "TYO", "SEA", "BKK", "AKL", "NYC" };

            foreach(var item in airportIds)
            {
                var airport = _airportService.Get(item);
                if(airport != null 
                    && _flightService.Get().Where(o => !o.FlightInfos.IsEmpty() && o.DepartureAirport == airport.Id && o.IsStay()).Count() > 0)
                {
                    airports.Add(airport);
                }
            }

            return airports.Select(o => new
            {
                Id = o.Id,
                Name = o.Name.Get("zh-CN"),
                Latitude = o.Latitude,
                Longitude = o.Longitude
            }).ToList();
        }
    }
}
