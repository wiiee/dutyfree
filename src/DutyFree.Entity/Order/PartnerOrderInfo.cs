namespace DutyFree.Entity.Order
{
    using Platform.Enum;
    using System.Collections.Generic;
    using System.Linq;
    using ValueType.Order;

    //带货端订单
    public class PartnerOrderInfo : BaseEntity
    {
        //Id: [UserId]_[Index]

        //机场
        public string AirportId { get; set; }

        //航班号
        public string FlightNo { get; set; }

        //订单产品
        public Dictionary<string, PartnerProductInfo> PartnerProductInfos;

        //该订单的状态
        //Initial:初始状态
        //Pending:接单了
        //Ongoing:买好了
        //Done:交货完成
        public Status Status { get; set; }

        public List<string> ReceiptImgIds { get; set; }

        //结算时的汇率
        public double Rate { get; set; }

        //订单实际价格
        public double Price { get; set; }

        //订单货币
        public CurrencySymbol Symbol { get; set; }

        //收益
        public double Earning { get; set; }

        public void AddReceiptImg(string imgId)
        {
            if(this.ReceiptImgIds == null)
            {
                this.ReceiptImgIds = new List<string>();
            }

            this.ReceiptImgIds.Add(imgId);
        }

        public PartnerOrderInfo()
        {
            this.PartnerProductInfos = new Dictionary<string, PartnerProductInfo>();
        }

        //差价获取50%的收益，正价获取10%的收益
        public double GetEarning(double price)
        {
            return 0;
        }

        public int GetCount()
        {
            return PartnerProductInfos.Sum(o => o.Value.Quantity);
        }
    }
}
