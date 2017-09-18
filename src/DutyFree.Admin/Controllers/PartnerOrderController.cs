namespace DutyFree.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using WebCore.Base;
    using Microsoft.AspNetCore.Authorization;

    [Authorize]
    public class PartnerOrderController : BaseController
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Item(string itemId)
        {
            ViewData["ItemId"] = itemId;
            return View();
        }
    }
}
