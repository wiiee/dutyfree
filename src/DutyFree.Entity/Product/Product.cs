namespace DutyFree.Entity.Product
{
    using Platform.Enum;
    using System;
    using System.Collections.Generic;
    using ValueType;

    public class Product : BaseEntity
    {
        //Id

        public string Name { get; set; }

        public string Description { get; set; }

        public List<string> DescriptionImgIds { get; set; }

        public string BrandId { get; set; }

        //规格
        public Specification Specification { get; set; }

        //public CompetitorInfo CompetitorInfo { get; set; }

        public List<CompetitorInfo> CompetitorInfos { get; set; }

        //类别
        public string CategoryId { get; set; }

        public string Logo { get; set; }

        public List<string> ImgIds { get; set; }

        //成本价，不同机场的价格
        public Dictionary<string, double> Costs { get; set; }

        //售卖价，不同机场的价格
        public Dictionary<string, double> Prices { get; set; }

        //成本价
        public double Cost { get; set; }

        //售价
        public double Price { get; set; }

        //参考价格
        public double ReferencePrice { get; set; }

        //最低价
        public double MinPrice { get; set; }

        //最高价
        public double MaxPrice { get; set; }

        public DateTime LastPriceTime { get; set; }

        //市场价
        public double MarketPrice { get; set; }

        //属性，一个产品可能有不同的属性，比如ML/OZ
        public List<ProductProperty> Properties { get; set; }

        public ProductStatus ProductStatus { get; set; }
    }
}
