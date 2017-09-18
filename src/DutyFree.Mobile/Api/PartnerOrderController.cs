namespace DutyFree.Mobile.Api
{
    using Entity.Order;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Platform.Enum;
    using Platform.Extension;
    using Platform.Model;
    using Service.Media;
    using Service.Order;
    using Service.Product;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using WebCore.Base;
    using WebCore.Extension;

    [Authorize]
    [Route("api/[controller]")]
    public class PartnerOrderController : BaseController
    {
        private PartnerOrderService _partnerOrderService;
        private PartnerProductService _partnerProductService;
        private ProductService _productService;
        private ImgService _imgService;

        public PartnerOrderController(PartnerOrderService partnerOrderService, PartnerProductService partnerProductService,
            ProductService productService, ImgService imgService)
        {
            this._partnerOrderService = partnerOrderService;
            this._partnerProductService = partnerProductService;
            this._productService = productService;
            this._imgService = imgService;
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
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

        [Route("GetPartnerOrderInfos")]
        [HttpPost]
        public List<PartnerOrderInfo> GetPartnerOrderInfos()
        {
            var partnerOrder = this._partnerOrderService.Get(this.GetUserId());
            if (partnerOrder != null && !partnerOrder.PartnerOrderInfos.IsEmpty())
            {
                return partnerOrder.PartnerOrderInfos;
            }
            else
            {
                return new List<PartnerOrderInfo>();
            }
        }

        [Route("GetProducts")]
        [HttpPost]
        public object GetProducts([FromBody]Page page)
        {
            var result = this._partnerProductService.GetProducts(this.GetUserId());

            if (result.IsEmpty())
            {
                return new List<string>();
            }

            var products = this._productService.GetByIds(new List<string>(result.Keys));

            return result.Select(o =>
            {
                var product = products.Find(p => p.Id == o.Key);
                return new
                {
                    ProductId = o.Key,
                    Quantity = o.Value,
                    Logo = product.Logo,
                    Name = product.Name,
                    MarketPrice = product.MarketPrice,
                    Price = product.Price,
                    ReferencePrice = product.ReferencePrice,
                    MinPrice = product.MinPrice,
                    MaxPrice = product.MaxPrice
                };
            }).ToList().GetPage(page);
        }

        [Route("AddCart")]
        [HttpGet]
        public void AddCart(string productId, int quantity)
        {
            this._partnerProductService.AddCart(this.GetUserId(), productId, quantity);
        }

        [Route("RemoveCart")]
        [HttpGet]
        public void RemoveCart(string productId, int quantity)
        {
            this._partnerProductService.RemoveCart(this.GetUserId(), productId, quantity);
        }

        [Route("GetActive")]
        [HttpGet]
        public object GetActive()
        {
            var orderInfo = this._partnerOrderService.GetActivePartnerOrderInfo(this.GetUserId());
            var result = orderInfo.PartnerProductInfos;

            if (!result.IsEmpty())
            {
                var products = this._productService.GetByIds(new List<string>(result.Keys));

                var items = result.Select(o =>
                {
                    var product = products.Find(p => p.Id == o.Key);
                    return new
                    {
                        ProductId = o.Key,
                        Quantity = o.Value.Quantity,
                        Logo = product.Logo,
                        Name = product.Name,
                        MarketPrice = product.MarketPrice,
                        Price = product.Price,
                        ReferencePrice = product.ReferencePrice,
                        MinPrice = product.MinPrice,
                        MaxPrice = product.MaxPrice
                    };
                }).ToList();

                return new
                {
                    Id = orderInfo.Id,
                    Items = items,
                    Status = (int)orderInfo.Status,
                    FlightNo = orderInfo.FlightNo,
                    AirportId = orderInfo.AirportId
                };
            }
            else
            {
                return new
                {
                    Id = orderInfo.Id,
                    Items = new List<string>(),
                    Status = (int)orderInfo.Status,
                    FlightNo = orderInfo.FlightNo,
                    AirportId = orderInfo.AirportId
                };
            }
        }

        [Route("SetFlightNo")]
        [HttpGet]
        public void SetFlightNo(string flightNo)
        {
            if (!string.IsNullOrEmpty(flightNo))
            {
                var partnerOrderInfo = _partnerOrderService.GetActivePartnerOrderInfo(this.GetUserId());
                partnerOrderInfo.FlightNo = flightNo;
                _partnerOrderService.UpdatePartnerOrderInfo(partnerOrderInfo);
            }
        }

        [Route("SetAirportId")]
        [HttpGet]
        public void SetAirportId(string airportId)
        {
            if (!string.IsNullOrEmpty(airportId))
            {
                var partnerOrderInfo = _partnerOrderService.GetActivePartnerOrderInfo(this.GetUserId());
                partnerOrderInfo.AirportId = airportId;
                _partnerOrderService.UpdatePartnerOrderInfo(partnerOrderInfo);
            }
        }

        [Route("BuyOrder")]
        [HttpGet]
        public void BuyOrder(string userId, string partnerOrderId, double price, CurrencySymbol currency)
        {

        }

        //// PUT api/values/5
        //[Route("UploadPartnerOrderReceipt")]
        //[HttpPut]
        //public void UploadPartnerOrderReceipt([FromBody]string[] imgs)
        //{
        //    var imgService = this.GetService<ImgService>();
        //    var partnerOrderService = this.GetService<PartnerOrderService>();
        //    var partnerOrderInfo = this.GetService<PartnerOrderService>().GetActivePartnerOrderInfo(this.GetUserId());

        //    var imgIds = new List<string>();

        //    foreach (var img in imgs)
        //    {
        //        byte[] bytes = Convert.FromBase64String(img);
        //        var pic = new Img(bytes.Length, partnerOrderInfo.Id, bytes, "image/jpeg");
        //        imgIds.Add(imgService.Create(pic));
        //    }

        //    partnerOrderInfo.ReceiptImgIds = imgIds;

        //    partnerOrderService.UpdatePartnerOrderInfo(partnerOrderInfo);
        //}

        // PUT api/values/5
        [Route("UploadPartnerOrderReceipt")]
        [HttpPost]
        public void UploadPartnerOrderReceipt(IFormFile file)
        {
            if (file != null)
            {
                Console.WriteLine(file.FileName);
                var partnerOrderInfo = _partnerOrderService.GetActivePartnerOrderInfo(this.GetUserId());

                var imgId = _imgService.SaveImg(file);

                if (!string.IsNullOrEmpty(imgId))
                {
                    partnerOrderInfo.AddReceiptImg(imgId);
                    //已经买好了
                    partnerOrderInfo.Status = Status.Ongoing;
                    _partnerOrderService.UpdatePartnerOrderInfo(partnerOrderInfo);
                }
            }
        }
    }
}
