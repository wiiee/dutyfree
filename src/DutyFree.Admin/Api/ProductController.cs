namespace DutyFree.Admin.Api
{
    using Entity.Product;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Platform.Extension;
    using Platform.Model.BootstrapFileInput;
    using Platform.Util;
    using Service.Media;
    using Service.Product;
    using System;
    using System.Collections.Generic;
    using WebCore.Base;

    [Route("api/[controller]")]
    public class ProductController : BaseController
    {
        private static ILogger _logger = LoggerUtil.CreateLogger<SettingController>();
        private ProductService _productService;
        private ImgService _imgService;

        public ProductController(ProductService productService, ImgService imgService)
        {
            this._productService = productService;
            this._imgService = imgService;
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<Product> Get()
        {
            return this._productService.Get();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public Product Get(string id)
        {
            return this._productService.Get(id);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]Product entity)
        {
            this._productService.Update(entity);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(string id, [FromBody]Product entity)
        {
            this._productService.Create(entity);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [Route("GetProducts")]
        [HttpPost]
        public List<Product> GetProducts([FromBody]List<string> productIds)
        {
            try
            {
                return this._productService.GetByIds(productIds);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new List<Product>();
            }
        }

        [Route("GetSendingData")]
        [HttpPost]
        public Dictionary<string, SendingData> GetSendingData(string productId)
        {
            try
            {
                var product = this._productService.Get(productId);
                return BuildSendingData(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        private Dictionary<string, SendingData> BuildSendingData(Product product)
        {
            var result = new Dictionary<string, SendingData>();

            var sendingData = new SendingData(new List<InitialPreview>(), new List<InitialPreviewConfig>());

            if (!string.IsNullOrEmpty(product.Logo))
            {
                var img = _imgService.Get(product.Logo);
                
                if (img != null)
                {
                    sendingData.initialPreviewConfigs.Add(new InitialPreviewConfig(img.Name, "120px", "/api/img/remove", img.Id, new { productId = product.Id }));
                    sendingData.initialPreviews.Add(new InitialPreview(img.Id, "file-preview-image", img.Name, img.Name).GetPreview());
                }
                result.Add("Logo", sendingData);
            }

            if (!product.ImgIds.IsEmpty())
            {
                var imgs = product.ImgIds;
                sendingData = new SendingData(new List<InitialPreview>(), new List<InitialPreviewConfig>());

                foreach (var id in imgs)
                {
                    var img = _imgService.Get(id);
                    if (img != null)
                    {
                        sendingData.initialPreviewConfigs.Add(new InitialPreviewConfig(img.Name, "120px", "/api/img/remove", img.Id, new { productId = product.Id }));
                        sendingData.initialPreviews.Add(new InitialPreview(img.Id, "file-preview-image", img.Name, img.Name).GetPreview());
                    }
                }

                result.Add("Imgs", sendingData);
            }

            return result;
        }
    }
}
