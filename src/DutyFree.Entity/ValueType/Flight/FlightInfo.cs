namespace DutyFree.Entity.ValueType.Flight
{
    using System.Collections.Generic;

    public class FlightInfo
    {
        //航班开始结束时间
        public Time DepartureTime { get; set; }
        public Time ArrivalTime { get; set; }

        public int DateOffset { get; set; }

        public string Airplane { get; set; }
        public string AirlineCode { get; set; }

        public string DepartureTerminal { get; set; }
        public string ArrivalTerminal { get; set; }

        public int Stopover { get; set; }
        public List<string> StopAirports { get; set; }

        public string CodeShareFlightNo { get; set; }

        public int Duration { get; set; }
    }
}
