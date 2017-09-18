namespace DutyFree.Mobile.Api
{
    using Entity.Product;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Platform.Extension;
    using Platform.Model;
    using Platform.Util;
    using Service.Product;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using WebCore.Base;

    [Route("api/[controller]")]
    public class ProductController : BaseController
    {
        private static ILogger _logger = LoggerUtil.CreateLogger<ProductController>();
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public Product Get(string id)
        {
            return _productService.Get(id);
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

        [Route("WithCategory")]
        [HttpPost]
        public object WithCategory(string categoryId)
        {
            var products = _productService.Get(o => o.CategoryId == categoryId);
            var random = new Random();

            return products.OrderBy(o => random.Next()).Take(50).Select(o => new
            {
                Id = o.Id,
                Description = o.Description,
                ImgIds = o.ImgIds,
                Price = Math.Round(o.Price, 2),
                Logo = o.Logo,
                BrandId = o.BrandId
            }).ToList();
        }

        [Route("WithBrand")]
        [HttpPost]
        public object WithBrand(string brandId, [FromBody]Page page)
        {
            var products = _productService.Get(o => o.BrandId == brandId);
            if(page != null && page.PageSize > 0)
            {
                products = products.GetPage(page);
            }

            return products.Select(o => new
            {
                Id = o.Id,
                Name = o.Name,
                Description = o.Description,
                DescriptionImgIds = o.DescriptionImgIds,
                ImgIds = o.ImgIds,
                Price = Math.Round(o.Price, 2),
                MarketPrice = Math.Round(o.MarketPrice),
                Logo = o.Logo,
                BrandId = o.BrandId
            }).ToList();
        }

        [Route("GetProducts")]
        [HttpPost]
        public object GetProducts([FromBody]List<string> productIds)
        {
            var products = _productService.GetByIds(productIds);

            return products.ToDictionary(o => o.Id, o => new
            {
                Id = o.Id,
                Name = o.Name,
                Description = o.Description,
                DescriptionImgIds = o.DescriptionImgIds,
                ImgIds = o.ImgIds,
                Price = Math.Round(o.Price, 2),
                MarketPrice = Math.Round(o.MarketPrice),
                ReferencePrice = Math.Round(o.ReferencePrice),
                MinPrice = Math.Round(o.MinPrice),
                MaxPrice = Math.Round(o.MaxPrice),
                Logo = o.Logo,
                BrandId = o.BrandId
            });
        }
    }
}
