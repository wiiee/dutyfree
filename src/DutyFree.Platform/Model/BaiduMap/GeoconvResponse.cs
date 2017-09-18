namespace DutyFree.Platform.Model.BaiduMap
{
    using System.Collections.Generic;

    public class GeoconvResponse
    {
        public int status { get; set; }
        public List<GeoconvResult> result { get; set; }
    }

    public class GeoconvResult
    {
        public double x { get; set; }
        public double y { get; set; }
    }

}
