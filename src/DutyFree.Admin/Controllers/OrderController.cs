namespace DutyFree.Admin.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Service.Order;
    using WebCore.Base;

    [Authorize]
    public class OrderController : BaseController
    {
        private OrderService _orderService;

        public OrderController(OrderService orderService)
        {
            this._orderService = orderService;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ConfirmOrder()
        {
            return View();
        }

        public IActionResult Confirm(string orderInfoId)
        {
            _orderService.PayOrderInfo(orderInfoId);
            return RedirectToAction("ConfirmOrder");
        }

        public IActionResult Item(string itemId)
        {
            ViewData["ItemId"] = itemId;
            return View();
        }
    }
}
