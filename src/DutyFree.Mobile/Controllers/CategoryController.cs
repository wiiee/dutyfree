namespace DutyFree.Mobile.Controllers
{
    using WebCore.Base;
    using Microsoft.AspNetCore.Mvc;

    public class CategoryController : BaseController
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetItem(string item)
        {
            return PartialView(item);
        }
    }
}
