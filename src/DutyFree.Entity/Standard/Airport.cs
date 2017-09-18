namespace DutyFree.Entity.Standard
{
    using ValueType;

    //机场
    public class Airport : BaseEntity
    {
        //Id为TLA

        public LocalField Name { get; set; }

        public string RegionId { get; set; }

        public string CountryId { get; set; }

        //纬度
        public double Latitude { get; set; }

        //经度
        public double Longitude { get; set; }
    }
}
