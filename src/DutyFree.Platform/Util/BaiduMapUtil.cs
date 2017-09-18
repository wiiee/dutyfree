namespace DutyFree.Platform.Util
{
    using Model.BaiduMap;
    using Newtonsoft.Json;

    public static class BaiduMapUtil
    {
        public static GeocoderSearchResponse GetGeo(double latitude, double longitude)
        {
            var url = string.Format("http://api.map.baidu.com/geocoder/v2/?ak={0}&location={1},{2}&output=json&coordtype=wgs84ll", "wavxymeawXnIqXO2HZcc8REWU3Py1A1e", latitude, longitude);
            return JsonConvert.DeserializeObject<GeocoderSearchResponse>(HttpUtil.GetString(url));
        }

        //from
        //取值为如下：
        //1：GPS设备获取的角度坐标，wgs84坐标;
        //2：GPS获取的米制坐标、sogou地图所用坐标;
        //3：google地图、soso地图、aliyun地图、mapabc地图和amap地图所用坐标，国测局坐标;
        //4：3中列表地图坐标对应的米制坐标;
        //5：百度地图采用的经纬度坐标;
        //6：百度地图采用的米制坐标;
        //7：mapbar地图坐标;
        //8：51地图坐标
        //to
        //有两种可供选择：5、6。
        //5：bd09ll(百度经纬度坐标),
        //6：bd09mc(百度米制经纬度坐标);
        public static GeoconvResponse GeoConvert(double latitude, double longitude, int from = 1, int to = 5)
        {
            var url = string.Format("http://api.map.baidu.com/geoconv/v1/?coords={0},{1}&from={2}&to={3}&ak={4}", latitude, longitude, from, to, "wavxymeawXnIqXO2HZcc8REWU3Py1A1e");
            return JsonConvert.DeserializeObject<GeoconvResponse>(HttpUtil.GetString(url));
        }
    }
}
