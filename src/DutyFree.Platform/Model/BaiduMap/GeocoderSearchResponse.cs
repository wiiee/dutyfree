namespace DutyFree.Platform.Model.BaiduMap
{
    using System.Collections.Generic;

    public class GeocoderSearchResponse
    {
        public int status { get; set; }
        public GeocoderSearchResult result { get; set; }
    }

    public class GeocoderSearchResult
    {
        public Location location { get; set; }
        public string formatted_address { get; set; }
        public string business { get; set; }
        public AddressComponent addressComponent { get; set; }
        public List<Poi> pois { get; set; }
        public string sematic_description { get; set; }
        public string cityCode { get; set; }
    }

    public class Location
    {
        public double lng { get; set; }
        public double lat { get; set; }
    }

    public class AddressComponent
    {
        public string country { get; set; }
        public string country_code { get; set; }
        public string province { get; set; }
        public string city { get; set; }
        public string district { get; set; }
        public string adcode { get; set; }
        public string street { get; set; }
        public string street_number { get; set; }
        public string direction { get; set; }
        public string distance { get; set; }
    }

    public class Poi
    {
        public string addr { get; set; }
        public string cp { get; set; }
        public string direction { get; set; }
        public string distance { get; set; }
        public string name { get; set; }
        public string poiType { get; set; }
    }
}
