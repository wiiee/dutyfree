namespace DutyFree.Entity.Standard
{
    using Platform.Extension;
    using System;
    using System.Collections.Generic;
    using ValueType.Flight;

    public class Flight : BaseEntity
    {
        //Id为航班的标识符

        //public LocalField Name { get; set; }

        //航班开始结束机场
        public string DepartureAirport { get; set; }
        public string ArrivalAirport { get; set; }

        public int DepartureTimeZone { get; set; }
        public int ArrivalTimeZone { get; set; }

        //周一到周日的航班信息
        public Dictionary<DayOfWeek, FlightInfo> FlightInfos { get; set; }

        public Flight(string id)
        {
            this.Id = id;
            this.FlightInfos = new Dictionary<DayOfWeek, FlightInfo>();
        }

        public FlightInfo GetCurrentFlightInfo()
        {
            var now = DateTime.UtcNow;
            now = now.AddHours(DepartureTimeZone);

            if (FlightInfos.ContainsKey(now.DayOfWeek))
            {
                return FlightInfos[now.DayOfWeek];
            }

            return null;
        }

        //是否今天的航班还在
        public bool IsStay()
        {
            if (FlightInfos.IsEmpty())
            {
                return false;
            }

            var now = DateTime.UtcNow;
            now = now.AddHours(DepartureTimeZone);
            var day = now.DayOfWeek;

            if (FlightInfos.ContainsKey(day))
            {
                var flightInfo = FlightInfos[day];
                var departureTime = flightInfo.DepartureTime;
                return departureTime.GetSeconds() > now.GetSeconds();
            }

            return false;
        }
    }
}
