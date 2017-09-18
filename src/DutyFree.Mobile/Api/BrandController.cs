namespace DutyFree.Mobile.Api
{
    using Entity.Standard;
    using Microsoft.AspNetCore.Mvc;
    using Service.Product;
    using Service.Standard;
    using System.Collections.Generic;
    using System.Linq;
    using WebCore.Base;

    [Route("api/[controller]")]
    public class BrandController : BaseController
    {
        private BrandService _brandService;
        private ProductService _productService;

        public BrandController(BrandService brandService, ProductService productService)
        {
            this._brandService = brandService;
            this._productService = productService;
        }
        // GET: api/values
        [HttpGet]
        public IEnumerable<Brand> Get()
        {
            return _brandService.Get();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public Brand Get(string id)
        {
            return _brandService.Get(id);
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

        [Route("GetBrands")]
        [HttpGet]
        public List<Brand> GetBrands(string categoryId)
        {
            if (string.IsNullOrEmpty(categoryId))
            {
                return _brandService.Get();
            }
            else
            {
                var brandIds = _productService.Get(o => o.CategoryId == categoryId).Select(o => o.BrandId).Distinct().ToList();
                return _brandService.GetByIds(brandIds);
            }
        }
    }
}
