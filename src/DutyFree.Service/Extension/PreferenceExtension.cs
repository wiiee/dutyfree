namespace DutyFree.Service.Extension
{
    using Context;
    using Entity.User;
    using Platform.Extension;
    using Product;

    public static class PreferenceExtension
    {
        public static double GetTotalPrice(this Preference preference)
        {
            if(preference == null || preference.Carts.IsEmpty())
            {
                return 0;
            }

            var productService = ServiceFactory.Instance.GetService<ProductService>();

            double result = 0;

            if(!preference.Carts.IsEmpty())
            {
                foreach (var item in preference.Carts)
                {
                    if (item.Value.IsSelected)
                    {
                        var product = productService.Get(item.Key);
                        result += product.Price * item.Value.Quantity;
                    }
                }
            }

            return result;
        }
    }
}
