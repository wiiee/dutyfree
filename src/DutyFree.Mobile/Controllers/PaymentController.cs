namespace DutyFree.Mobile.Controllers
{
    using WebCore.Base;
    using Microsoft.AspNetCore.Mvc;
    using Platform.Util.Domain;
    using Service.Order;

    public class PaymentController : BaseController
    {
        private OrderService _orderService;

        public PaymentController(OrderService orderService)
        {
            this._orderService = orderService;
        }

        // GET: /<controller>/
        public IActionResult Index(string orderInfoId)
        {
            ViewData["OrderInfo"] = this._orderService.GetOrderInfo(orderInfoId);
            ViewData["Order"] = this._orderService.Get(OrderUtil.GetUserId(orderInfoId));
            return View();
        }
    }
}
