namespace DutyFree.Mobile.Controllers
{
    using WebCore.Base;
    using WebCore.Extension;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Service.Order;

    [Authorize]
    public class PlaceOrderController : BaseController
    {
        private OrderService _orderService;

        public PlaceOrderController(OrderService orderService)
        {
            this._orderService = orderService;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            var orderInfoId = this._orderService.CreateOrderInfo(this.GetUserId());
            return Redirect("~/Payment?orderInfoId=" + orderInfoId);
        }
    }
}
