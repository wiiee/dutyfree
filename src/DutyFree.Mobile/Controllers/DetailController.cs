namespace DutyFree.Mobile.Controllers
{
    using WebCore.Base;
    using Microsoft.AspNetCore.Mvc;
    using Service.Product;

    public class DetailController : BaseController
    {
        private static string[] TAB_NAMES = { "Product", "Detail", "Review" };

        private ProductService _productService;

        public DetailController(ProductService productService)
        {
            this._productService = productService;
        }

        // GET: /<controller>/
        public IActionResult Index(string productId)
        {
            ViewData["Product"] = this._productService.Get(productId);
            return View();
        }

        // GET: /<controller>/
        public IActionResult Provider(string productId)
        {
            ViewData["Product"] = this._productService.Get(productId);
            return View();
        }

        public IActionResult GetTab(string productId, int tab)
        {
            ViewData["Product"] = this._productService.Get(productId);

            return PartialView(TAB_NAMES[tab]);
        }
    }
}
