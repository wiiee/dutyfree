namespace DutyFree.Mobile.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using WebCore.Base;

    public class AddressController : BaseController
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        // GET: /<controller>/
        public IActionResult Add()
        {
            return View();
        }

        // GET: /<controller>/
        public IActionResult Edit(int index)
        {
            ViewData["Index"] = index;
            return View();
        }
    }
}
