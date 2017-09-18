namespace DutyFree.Mobile.Controllers
{
    using Service.User;
    using WebCore.Base;

    public class AccountController : BaseAccountController
    {
        public AccountController(UserService userService) : base(userService) { }
    }
}
