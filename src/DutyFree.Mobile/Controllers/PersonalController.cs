namespace DutyFree.Mobile.Controllers
{
    using WebCore.Base;
    using WebCore.Extension;
    using Entity.Order;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Platform.Enum;
    using Service.Order;
    using System.Collections.Generic;
    using System.Linq;

    [Authorize]
    public class PersonalController : BaseController
    {
        private OrderService _orderService;

        public PersonalController(OrderService orderService)
        {
            this._orderService = orderService;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Order(int type)
        {
            ViewData["Type"] = type;
            var order = this._orderService.Get(this.GetUserId());
            var orderInfos = order == null ? new List<OrderInfo>() : order.OrderInfos;

            if (type == 0)
            {
                ViewData["Title"] = "待付款";
                ViewData["OrderInfos"] = orderInfos.Where(o => o.IsUnpaid()).ToList();
            }
            else if (type == 1)
            {
                ViewData["Title"] = "待收货";
                ViewData["OrderInfos"] = orderInfos.Where(o => o.IsOngoing()).ToList();
            }
            else if(type == 3)
            {
                ViewData["Title"] = "已完成";
                ViewData["OrderInfos"] = orderInfos.Where(o => o.IsDone()).ToList();
            }

            return View();
        }

        public IActionResult PartnerOrder(int type)
        {
            return View();
        }

        public IActionResult OrderInfo(string orderInfoId)
        {
            ViewData["OrderInfoId"] = orderInfoId;
            return View();
        }

        public IActionResult Review(int type)
        {
            ViewData["Type"] = type;
            var order = _orderService.Get(this.GetUserId());
            var orderInfos = order == null ? new List<OrderInfo>() : order.OrderInfos;

            if (type == 0)
            {
                ViewData["Title"] = "待评价";
                ViewData["Items"] = orderInfos.SelectMany(o => o.GetPendingReviewItems()).ToList();
            }
            else if (type == 1)
            {
                ViewData["Title"] = "已评价";
                ViewData["Items"] = orderInfos.SelectMany(o => o.GetDoneReviewItems()).ToList();
            }

            return View();
        }

        public IActionResult Message()
        {
            return View();
        }

        public IActionResult Wallet()
        {
            return View();
        }
    }
}
