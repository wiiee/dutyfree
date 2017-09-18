namespace DutyFree.Mobile.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using WebCore.Base;
    using Service.Product;

    public class ReviewController : BaseController
    {
        private ProductService _productService;

        public ReviewController(ProductService productService)
        {
            this._productService = productService;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        // GET: /<controller>/
        public IActionResult Add(string orderInfoId, string productId)
        {
            ViewData["OrderInfoId"] = orderInfoId;
            ViewData["ProductId"] = productId;
            ViewData["Product"] = this._productService.Get(productId);

            return View();
        }
    }
}
