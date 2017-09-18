namespace DutyFree.WebCore.Extension
{
    using Microsoft.AspNetCore.Mvc.Razor;
    using Platform.Enum;
    using Platform.Util;
    using Service;
    using Service.Context;
    using System;
    using System.Globalization;
    using System.Security.Claims;

    public static class RazorPageExtension
    {
        public static bool IsLogIn(this RazorPage page)
        {
            var isAuthenticated = page.User?.Identity?.IsAuthenticated;
            return isAuthenticated == null ? false : (bool)isAuthenticated;
        }

        public static string GetUserId(this RazorPage page)
        {
            if (page.IsLogIn())
            {
                return page.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            }

            return string.Empty;
        }

        public static string GetUserName(this RazorPage page)
        {
            if (page.IsLogIn())
            {
                return page.User.FindFirst(ClaimTypes.Name).Value;
            }

            return string.Empty;
        }

        public static UserType GetUserType(this RazorPage page)
        {
            if (page.IsLogIn())
            {
                return (UserType)int.Parse(page.User.FindFirst(ClaimTypes.Role).Value);
            }

            return UserType.User;
        }

        public static string GetSessionValue(this RazorPage page, string key)
        {
            byte[] value;

            page.Context.Session.TryGetValue(key, out value);

            return StringUtil.ByteArrayToStr(value);
        }

        public static T GetService<T>(this RazorPage page)
            where T : IService
        {
            var type = typeof(T);

            var context = new ServiceContext("View", string.Empty, false);

            return (T)Activator.CreateInstance(type, context);
        }

        public static bool IsMobileBrowser(this RazorPage page)
        {
            return page.Context.Request.IsMobileBrowser();
        }

        public static string GetCurrencySymbol(this RazorPage page)
        {
            return RegionInfo.CurrentRegion.CurrencySymbol;
        }
    }
}
