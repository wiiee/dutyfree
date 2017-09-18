namespace DutyFree.Mobile.Controllers
{
    using WebCore.Base;
    using WebCore.Extension;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Platform.Enum;
    using Service.Order;

    [Authorize]
    public class CartController : BaseController
    {
        private PartnerOrderService _partnerOrderService;

        public CartController(PartnerOrderService partnerOrderService)
        {
            this._partnerOrderService = partnerOrderService;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Partner()
        {
            return View();
        }

        //添加到待买列表
        // GET: /<controller>/
        public IActionResult AcceptOrder(string partnerOrderId, string flightNo)
        {
            var userId = this.GetUserId();
            return RedirectToAction("Partner");
        }

        //买好单了
        public IActionResult BuyOrder(string partnerOrderId, double price, CurrencySymbol symbol)
        {
            var userId = this.GetUserId();
            return RedirectToAction("Partner");
        }

        //取消订单
        public IActionResult CancelOrder(string partnerOrderId)
        {
            var userId = this.GetUserId();
            return Redirect("~/Home/Provider");
        }
    }
}
