namespace DutyFree.Entity.ValueType
{
    public class Time
    {
        public int Hour { get; set; }
        public int Minute { get; set; }
        public int Second { get; set; }

        public Time(int hour, int minute, int second)
        {
            this.Hour = hour;
            this.Minute = minute;
            this.Second = second;
        }

        public int GetSeconds()
        {
            return Hour * 3600 + Minute * 60 + Second;
        }
    }
}
