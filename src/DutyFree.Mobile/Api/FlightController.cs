namespace DutyFree.Mobile.Api
{
    using Entity.Standard;
    using Microsoft.AspNetCore.Mvc;
    using Platform.Extension;
    using Service.Result;
    using Service.Standard;
    using System.Collections.Generic;
    using System.Linq;
    using WebCore.Base;

    [Route("api/[controller]")]
    public class FlightController : BaseController
    {
        private FlightService _flightService;

        public FlightController(FlightService flightService)
        {
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
        public Flight Get(string id)
        {
            return this._flightService.Get(id);
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

        [Route("GetFlights")]
        [HttpGet]
        public object GetFlights(string airportId)
        {
            var flights = _flightService.Get();
            flights = flights.Where(o => !o.FlightInfos.IsEmpty() && o.DepartureAirport == airportId && o.IsStay()).ToList();

            return flights.Select(o =>
            {
                var flightInfo = o.GetCurrentFlightInfo();
                return new
                {
                    Id = o.Id,
                    DepartureAirport = o.DepartureAirport,
                    ArrivalAirport = o.ArrivalAirport,
                    DepartureTime = flightInfo.DepartureTime,
                    ArrivalTime = flightInfo.ArrivalTime
                };
            }).ToList();
        }

        [Route("IsFlightValid")]
        [HttpGet]
        public ServiceResult IsFlightValid(string flightNo)
        {
            var result = new ServiceResult();
            //首先校验时间
            if (_flightService.IsTimeValid(flightNo))
            {
                result.IsSuccessful = true;
            }
            else
            {
                result.IsSuccessful = false;
                result.Msg = _flightService.GetErrorMsg(flightNo);
            }

            return result;
        }
    }
}
