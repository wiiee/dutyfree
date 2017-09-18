namespace DutyFree.Service.Extension
{
    using AngleSharp.Parser.Html;
    using Context;
    using Entity.Product;
    using Platform.Extension;
    using Platform.Util;
    using Product;
    using System;

    public static class ProductExtension
    {
        private static HtmlParser parser = new HtmlParser();

        public static double GetPrice(this Product product)
        {
            var productService = ServiceFactory.Instance.GetService<ProductService>();

            var now = DateTime.Now;

            if (product.LastPriceTime.IsEmpty() || (now - product.LastPriceTime).Days > 3)
            {
                try
                {
                    var price = GetCurrentPrice(product.CompetitorInfos[0].ProductId);

                    if(price > 0)
                    {
                        product.Price = price;
                        product.LastPriceTime = DateTime.Now;
                        productService.Update(product);

                        return product.Price;
                    }
                }
                catch (Exception)
                {
                    return product.MarketPrice;
                }
            }

            return product.Price;
        }

        private static double GetCurrentPrice(string productId)
        {
            try
            {
                double price = 0;
                var url = "http://www.kaola.com/product/" + productId + ".html";

                var content = HttpUtil.GetString(url);

                var document = parser.Parse(content);
                double.TryParse(document.QuerySelector("#js_currentPrice").TextContent.Substring(1), out price);

                if (price == 0)
                {
                    var startIndex = content.IndexOf("\"actualCurrentPrice\"");
                    if (startIndex != -1)
                    {
                        var endIndex = content.IndexOf(",", startIndex);

                        if (endIndex != -1)
                        {
                            double.TryParse(content.Substring(startIndex + 22, endIndex - startIndex - 23), out price);
                        }
                    }
                }

                return price;
            }
            catch
            {
                return 0;
            }
        }
    }
}
