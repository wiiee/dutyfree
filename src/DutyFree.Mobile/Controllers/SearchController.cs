namespace DutyFree.Mobile.Controllers
{
    using WebCore.Base;
    using Entity.Product;
    using Microsoft.AspNetCore.Mvc;
    using Service.Product;
    using Service.Standard;
    using System.Collections.Generic;
    using System.Linq;

    public class SearchController : BaseController
    {
        private ProductService _productService;
        private BrandService _brandService;

        public SearchController(ProductService productService, BrandService brandService)
        {
            this._productService = productService;
            this._brandService = brandService;
        }

        // GET: /<controller>/
        public IActionResult Index(string brandId, string text)
        {
            var products = new List<Product>();

            if (string.IsNullOrEmpty(brandId))
            {
                products = _productService.Get();
            }
            else
            {
                products = _productService.Get(o => o.BrandId == brandId);
            }

            if (!string.IsNullOrEmpty(text))
            {
                text = text.Trim();
                products = products.Where(o => IsSatisfied(o, text)).ToList();
            }

            ViewData["Products"] = products;
            ViewData["Text"] = text;
            return View();
        }

        private bool IsSatisfied(Product product, string text)
        {
            if (product.Name.Contains(text))
            {
                return true;
            }

            if (!string.IsNullOrEmpty(product.Description) && product.Description.Contains(text))
            {
                return true;
            }

            var brand = this._brandService.Get(product.BrandId);

            if (brand == null)
            {
                return false;
            }

            if (brand.Name != null && !string.IsNullOrEmpty(brand.Name.GetLocal()) && brand.Name.GetLocal().Contains(text))
            {
                return true;
            }

            return false;
        }
    }
}
