namespace DutyFree.Service.Standard
{
    using Entity.Standard;
    using Platform.Context;
    using Platform.Extension;
    using System;

    public class FlightService : BaseService<Flight>
    {
        private const int NO_FLIGHT = 100000;
        public FlightService(IContextRepository contextRepository) : base(contextRepository) { }

        public bool IsTimeValid(string flightNo)
        {
            var interval = GetInterval(flightNo);

            return interval > 0 && interval < 14400;
        }

        public int GetInterval(string flightNo)
        {
            var flight = Get(flightNo);

            if (flight != null && !flight.FlightInfos.IsEmpty())
            {
                var now = DateTime.UtcNow;
                now = now.AddHours(flight.DepartureTimeZone);
                var day = now.DayOfWeek;

                if (flight.FlightInfos.ContainsKey(day))
                {
                    var flightInfo = flight.FlightInfos[day];
                    var departureTime = flightInfo.DepartureTime;
                    return departureTime.GetSeconds() - now.GetSeconds();
                }
                else
                {
                    return NO_FLIGHT;
                }
            }
            else
            {
                return NO_FLIGHT;
            }
        }

        public string GetErrorMsg(string flightNo)
        {
            if (!IsTimeValid(flightNo))
            {
                var interval = GetInterval(flightNo);

                if(interval == NO_FLIGHT)
                {
                    return "今天没有该航班" + flightNo;
                }
                else if (interval < 0)
                {
                    return "飞机已经起飞了,起飞时间是" + GetDepartureTime(flightNo);
                }
                else
                {
                    return "飞机起飞时间是" + GetDepartureTime(flightNo) + ", 你的时间太早了, 请在起飞前四个小时再检查。";
                }
            }

            return string.Empty;
        }

        public string GetDepartureTime(string flightNo)
        {
            var flight = Get(flightNo);

            if (flight != null)
            {
                var now = DateTime.Now;
                var day = now.DayOfWeek;

                if (flight.FlightInfos.ContainsKey(day))
                {
                    var departureTime = flight.FlightInfos[day].DepartureTime;
                    return departureTime.Hour.ToString("D2") + ":" + departureTime.Minute.ToString("D2");
                }
            }

            return string.Empty;
        }
    }
}
