namespace DutyFree.WebCore.Base
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Platform.Util;
    using Service.Product;
    using Service.User;

    public abstract class BaseController : Controller
    {
        private static ILogger _logger = LoggerUtil.CreateLogger<BaseController>();

        //public UserService _userService;
        //public PreferenceService _preferenceService;
        //public ProductService _productService;
    }
}
