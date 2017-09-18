namespace DutyFree.Entity.ValueType
{
    using Platform.Enum;

    public class CompetitorInfo
    {
        //产品Id
        public string ProductId { get; set; }
        public Competitor Competitor { get; set; }
        public double Price { get; set; }
        public double MarketPrice { get; set; }

        public CompetitorInfo(string productId, Competitor competitor, double price, double marketPrice)
        {
            this.ProductId = productId;
            this.Competitor = competitor;
            this.Price = price;
            this.MarketPrice = marketPrice;
        }
    }
}
