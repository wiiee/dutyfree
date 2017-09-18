namespace DutyFree.Service.Extension
{
    using Context;
    using Entity.Order;
    using Product;

    public static class PartnerOrderInfoExtenstion
    {
        public static double GetTotalPrice(this PartnerOrderInfo partnerOrderInfo)
        {
            var productService = new ProductService(ServiceContextRepository.Instance);
            double result = 0;

            foreach(var item in partnerOrderInfo.PartnerProductInfos)
            {
                result += productService.Get(item.Key).Price * item.Value.Quantity;
            }

            return result;
        }
    }
}
