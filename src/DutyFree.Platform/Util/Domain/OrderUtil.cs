using DutyFree.Platform.Constant;

namespace DutyFree.Platform.Util.Domain
{
    public static class OrderUtil
    {
        public static string GetUserId(string orderId)
        {
            if (string.IsNullOrEmpty(orderId) || orderId.LastIndexOf("_") == -1)
            {
                return string.Empty;
            }

            return orderId.Substring(0, orderId.LastIndexOf("_"));
        }

        public static bool IsReady(double price)
        {
            return OrderConstant.PRICE_MAX - price < 500;
        }
    }
}
