namespace DutyFree.Admin.Controllers
{
    using WebCore.Base;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class SettingController : BaseController
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Category()
        {
            return View();
        }

        public IActionResult Property()
        {
            return View();
        }

        public IActionResult Unit()
        {
            return View();
        }

        public IActionResult Brand()
        {
            return View();
        }

        public IActionResult Airport()
        {
            return View();
        }

        public IActionResult Region()
        {
            return View();
        }
    }
}
