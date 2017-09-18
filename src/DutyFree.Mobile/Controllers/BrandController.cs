namespace DutyFree.Mobile.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Service.Product;
    using Service.Standard;
    using System.Linq;
    using WebCore.Base;

    public class BrandController : BaseController
    {
        private BrandService _brandService;
        private ProductService _productService;

        public BrandController(BrandService brandService, ProductService productService)
        {
            this._brandService = brandService;
            this._productService = productService;
        }

        // GET: /<controller>/
        public IActionResult Index(string brandId, string categoryId)
        {
            
            ViewData["Brand"] = _brandService.Get(brandId);
            var products = _productService.Get(o => o.BrandId == brandId);

            if (!string.IsNullOrEmpty(categoryId))
            {
                products = products.Where(o => o.CategoryId == categoryId).ToList();
            }

            ViewData["Products"] = products;
            return View();
        }

        public IActionResult GetPage(string brandId, int pageIndex, int pageSize)
        {
            var products = _productService.Get(o => o.BrandId == brandId);

            products = products.Skip(pageIndex * pageSize).Take(pageSize).ToList();

            ViewData["Products"] = products;

            return PartialView("Product/Items");
        }
    }
}
