namespace DutyFree.Mobile.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using WebCore.Base;
    using Microsoft.AspNetCore.Authorization;

    [Authorize]
    public class ConfirmOrderController : BaseController
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        // GET: /<controller>/
        public IActionResult Products()
        {
            return View();
        }
    }
}
