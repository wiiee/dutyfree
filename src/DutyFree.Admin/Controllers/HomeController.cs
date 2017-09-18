namespace DutyFree.Admin.Controllers
{
    using Entity.Media;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Model;
    using Newtonsoft.Json;
    using Platform.Util;
    using Service.Extension;
    using Service.Media;
    using Service.Product;
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using WebCore.Base;

    [Authorize]
    public class HomeController : BaseController
    {
        private static ILogger _logger = LoggerUtil.CreateLogger<HomeController>();
        private ProductService _productService;
        private ImgService _imgService;

        public HomeController(ProductService productService, ImgService imgService)
        {
            this._productService = productService;
            this._imgService = imgService;
        }

        public IActionResult Index()
        {
            //var text = OcrUtil.GetText();
            return View();
        }

        public string RefreshPrice()
        {
            var products = _productService.Get();

            products.ForEach(o => o.GetPrice());

            return "Refresh prices successfully!";
        }

        public string ImportBrands()
        {
            //BrandHelper.ImportBrands();

            return "Import brands successfully!";
        }

        public string ImportProducts()
        {
            //ProductHelper.ImportProducts();

            return "Import products successfully!";
        }

        public string CleanDuplicate()
        {
            //ProductHelper.CleanDuplicate();

            return "Clean successfully!";
        }

        public string SaveImgFile()
        {
            //ProductHelper.SaveImgFile();
            return "Save successfully!";
        }

        public string ResetProducts()
        {
            //ProductHelper.ResetProducts();

            return "Reset successfully!";
        }

        public string Flight()
        {
            //var flightService = this.GetService<FlightService>();

            //var filePath = @"E:\airport\FlightBaseInfos(New).txt";
            //var line = string.Empty;
            //string sep = "\t";
            //using (StreamReader r = new StreamReader(filePath))
            //{
            //    while ((line = r.ReadLine()) != null)
            //    {
            //        var strs = line.Split(sep.ToCharArray());

            //        var flightNo = strs[2].Trim();

            //        var flight = flightService.Get(flightNo);

            //        if(flight == null)
            //        {
            //            flight = new Flight(flightNo);
            //            flightService.Create(flight);
            //        }

            //        var effectDays = strs[3].ToCharArray();

            //        var flightInfo = new FlightInfo();

            //        flightInfo.Airplane = strs[11].Trim();
            //        flightInfo.AirlineCode = strs[1];

            //        flight.DepartureAirport = strs[4].Trim();
            //        flight.ArrivalAirport = strs[7].Trim();

            //        flightInfo.DepartureTime = new Time(int.Parse(strs[8]) / 100, int.Parse(strs[8]) % 100, 0);
            //        flightInfo.ArrivalTime = new Time(int.Parse(strs[9]) / 100, int.Parse(strs[9]) % 100, 0);

            //        flightInfo.DateOffset = int.Parse(strs[10]);

            //        flightInfo.DepartureTerminal = strs[12];
            //        flightInfo.ArrivalTerminal = strs[13];

            //        flightInfo.Stopover = int.Parse(strs[14]);
            //        flightInfo.StopAirports = string.IsNullOrEmpty(strs[15]) || strs[15] == "NULL" ? null : new List<string>(strs[15].Split(','));

            //        flightInfo.CodeShareFlightNo = strs[17];

            //        flight.DepartureTimeZone = int.Parse(strs[18]) / 100;
            //        flight.ArrivalTimeZone = int.Parse(strs[19]) / 100; ;

            //        flightInfo.Duration = int.Parse(strs[16]);

            //        foreach (var item in effectDays)
            //        {
            //            var day = int.Parse(item.ToString());
            //            var dayOfWeek = (DayOfWeek)(day == 7 ? 0 : day);
            //            flight.FlightInfos[dayOfWeek] = flightInfo;
            //        }

            //        flightService.Update(flight);
            //    }
            //}

            return "Import successfully";
        }

        public string Airport()
        {
            //var airportService = this.GetService<AirportService>();

            //var filePath = @"E:\airport\Airports-aus.txt";
            //var line = string.Empty;
            //string sep = "\t";
            //using (StreamReader r = new StreamReader(filePath))
            //{
            //    while((line = r.ReadLine()) != null)
            //    {
            //        var strs = line.Split(sep.ToCharArray());

            //        Airport airport = new Airport();
            //        airport.Id = strs[0];
            //        LocalField name = new LocalField(strs[1]);
            //        name.Add("en-US", strs[1]);
            //        name.Add("zh-CN", strs[3]);

            //        airport.Name = name;
            //        airport.CountryId = strs[2];

            //        airportService.Create(airport);
            //    }
            //}

            return "Import successfully";
        }

        public string TestJson()
        {
            var content = "{\"items\":[{\"id\":2816,\"name\":\"\\u5bc6\\u4e91\\u533a\"},{\"id\":72,\"name\":\"\\u671d\\u9633\\u533a\"},{\"id\":2901,\"name\":\"\\u660c\\u5e73\\u533a\"},{\"id\":2953,\"name\":\"\\u5e73\\u8c37\\u533a\"},{\"id\":2800,\"name\":\"\\u6d77\\u6dc0\\u533a\"},{\"id\":2801,\"name\":\"\\u897f\\u57ce\\u533a\"},{\"id\":2802,\"name\":\"\\u4e1c\\u57ce\\u533a\"},{\"id\":2803,\"name\":\"\\u5d07\\u6587\\u533a\"},{\"id\":2804,\"name\":\"\\u5ba3\\u6b66\\u533a\"},{\"id\":2805,\"name\":\"\\u4e30\\u53f0\\u533a\"},{\"id\":2806,\"name\":\"\\u77f3\\u666f\\u5c71\\u533a\"},{\"id\":2807,\"name\":\"\\u95e8\\u5934\\u6c9f\"},{\"id\":2808,\"name\":\"\\u623f\\u5c71\\u533a\"},{\"id\":2809,\"name\":\"\\u901a\\u5dde\\u533a\"},{\"id\":3065,\"name\":\"\\u5ef6\\u5e86\\u53bf\"},{\"id\":2810,\"name\":\"\\u5927\\u5174\\u533a\"},{\"id\":2812,\"name\":\"\\u987a\\u4e49\\u533a\"},{\"id\":2814,\"name\":\"\\u6000\\u67d4\\u533a\"}]}";

            try
            {
                var regions = JsonConvert.DeserializeObject<Regions>(content);
                return "Test successfully!";
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }

        public string GetPinYin()
        {
            //var str1 = PinYinUtil.GetPinyin("测试一下");
            //var str2 = UnicodeUtil.DecodeEncodedNonAsciiCharacters("\u5bc6\u4e91\u533a");
            //Console.WriteLine(str2);

            ImportRegions();

            return "Import successfully!";
        }

        public string Test()
        {
            GetProducts("Clinique", "Cosmestics", "Clinique");
            GetProducts("Loreal", "Cosmestics", "Loreal");
            GetProducts("Estée Lauder", "Cosmestics", "Estée Lauder");
            return "Download successfully";
        }

        private string GetElement(string content, string start, string end = null)
        {
            var startIndex = content.IndexOf(start);
            var endIndex = 0;

            if(end != null)
            {
                endIndex = content.IndexOf(end, startIndex + start.Length);
            }
            
            if(end == null)
            {
                return content.Substring(startIndex + start.Length);
            }

            return content.Substring(startIndex + start.Length, endIndex - startIndex - start.Length);
        }

        private string SaveImg(string url)
        {
            var img = new Img();
            var startIndex = url.LastIndexOf('/');
            img.Name = url.Substring(startIndex + 1);

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                var response = client.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    img.Length = (int)response.Content.Headers.ContentLength;
                    img.ContentType = response.Content.Headers.ContentType.MediaType;
                    img.Content = response.Content.ReadAsByteArrayAsync().Result;

                    return _imgService.Create(img);
                    //do something with the response here. Typically use JSON.net to deserialise it and work with it
                }
                else
                {
                    return null;
                }
            }
        }

        private string GetProducts(string brandId, string categoryId, string fileName)
        {
            //List<string> lis = new List<string>();
            //List<Product> products = new List<Product>();
            //var filePath = string.Format(@"E:\DutyFree\Jd\{0}.txt", fileName);

            //using (StreamReader r = new StreamReader(filePath))
            //{
            //    var content = r.ReadToEnd();
            //    var index = 0;
            //    var startIndex = 0;
            //    while((startIndex = content.IndexOf("<li", index)) != -1)
            //    {
            //        var endIndex = content.IndexOf("</li>", startIndex);
            //        index = endIndex;
            //        lis.Add(content.Substring(startIndex, endIndex - startIndex + 1));
            //    }
            //}

            //foreach(var li in lis)
            //{
            //    try
            //    {
            //        var product = new Product();
            //        product.BrandId = brandId;
            //        product.CategoryId = categoryId;

            //        var img = GetElement(li, "<img", ">");
            //        var url = string.Empty;

            //        if (img.IndexOf("src=\"") != -1)
            //        {
            //            url = "http:" + GetElement(img, "src=\"", "\"");
            //        }
            //        else if (img.IndexOf("data-lazy-img=\"") != -1)
            //        {
            //            url = "http:" + GetElement(img, "data-lazy-img=\"", "\"");
            //        }

            //        product.Logo = SaveImg(url);

            //        var cutLi = li.Substring(li.IndexOf("</em>"));
            //        var name = GetElement(cutLi, "<em>", "</em>");
            //        product.Name = name;

            //        var price = GetElement(li, "data-price=\"", "\"");

            //        var dataSku = GetElement(li, "data-sku=\"", "\"");

            //        CompetitorInfo info = new CompetitorInfo(dataSku, Competitor.Jd, double.Parse(price), double.Parse(price));

            //        product.Cost = double.Parse(price);
            //        product.ImgIds = CreateImgs(dataSku);

            //        product.CompetitorInfos = new List<CompetitorInfo> { info };

            //        products.Add(product);
            //    }
            //    catch (Exception ex)
            //    {
            //        _logger.LogInformation(ex.Message);
            //    }
            //}

            //this.GetService<ProductService>().Create(products);

            return string.Empty;
        }

        private List<string> getImgs(string content)
        {
            var result = new List<string>();
            var index = 0;
            var startIndex = 0;
            while ((startIndex = content.IndexOf("data-url='", index)) != -1)
            {
                var endIndex = content.IndexOf("'", startIndex + 12);
                index = endIndex;
                result.Add("http://img14.360buyimg.com/n1/s448x448_" + content.Substring(startIndex + 10, endIndex - startIndex - 10));
            }

            return result;
        }

        private List<string> CreateImgs(string dataSku)
        {
            var url = string.Format("http://item.jd.hk/{0}.html", dataSku);
            var result = new List<string>();
            var urls = new List<string>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                var response = client.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    var content = response.Content.ReadAsStringAsync().Result;
                    urls = getImgs(content);
                    //do something with the response here. Typically use JSON.net to deserialise it and work with it
                }
            }

            foreach(var item in urls)
            {
                result.Add(SaveImg(item));
            }
            
            return result;
        }

        private void ImportRegions()
        {
            //var filePath = @"E:\airport\regions.txt";
            //var regionService = this.GetService<RegionService>();

            //using (StreamReader r = new StreamReader(filePath))
            //{
            //    var line = string.Empty;
            //    while ((line = r.ReadLine()) != null)
            //    {
            //        var items = line.Split(',');
            //        var name = items[0];
            //        var id = int.Parse(items[1]);

            //        Region region = new Region();
            //        region.ParentId = "China";
            //        region.Id = PinYinUtil.GetPinyin(name);
            //        region.Name = new LocalField(name);

            //        try
            //        {
            //            regionService.Create(region);
            //        }
            //        catch(Exception ex)
            //        {
            //            Console.WriteLine(ex.Message);
            //        }

            //        BuildRegion(id, region.Id);
            //    }
            //}
        }

        private void BuildRegion(int id, string parentId)
        {
            //try
            //{
            //    var regionService = this.GetService<RegionService>();
            //    var url = string.Format("http://d.jd.com/area/get?fid={0}&callback=getAreaListcallback", id);

            //    using (var client = new HttpClient())
            //    {
            //        client.BaseAddress = new Uri(url);
            //        client.DefaultRequestHeaders.Accept.Clear();
            //        var response = client.GetAsync(url).Result;
            //        if (response.IsSuccessStatusCode)
            //        {
            //            var content = response.Content.ReadAsStringAsync().Result;

            //            if (!string.IsNullOrEmpty(content))
            //            {
            //                content = content.Substring(20);
            //                content = content.Substring(0, content.Length - 1);
            //                content = "{\"items\":" + content;
            //                content += "}";
            //                var regions = JsonConvert.DeserializeObject<Regions>(content);

            //                foreach (var item in regions.items)
            //                {
            //                    Region region = new Region();
            //                    region.ParentId = parentId;
            //                    var pinYin = PinYinUtil.GetPinyin(item.name);
            //                    //var name = UnicodeUtil.DecodeEncodedNonAsciiCharacters(item.name);
            //                    if(regionService.Get(pinYin) != null)
            //                    {
            //                        for (int i = 2; i < 1000; i++)
            //                        {
            //                            var newId = pinYin + i;
            //                            if (regionService.Get(newId) == null)
            //                            {
            //                                pinYin = newId;
            //                                break;
            //                            }
            //                        }
            //                    }

            //                    region.Id = pinYin;
            //                    region.Name = new LocalField(item.name);

            //                    regionService.Create(region);

            //                    BuildRegion(item.id, region.Id);
            //                }
            //            }
            //            //do something with the response here. Typically use JSON.net to deserialise it and work with it
            //        }
            //    }
            //}
            //catch(Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}
        }
    }
}
