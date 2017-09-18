namespace DutyFree.Entity.ValueType.User
{
    using Platform.Enum;
    using System;

    public class WalletItem
    {
        public RewardType RewardType { get; set; }
        public string OrderInfoId { get; set; }
        public double Number { get; set; }
        public DateTime Time { get; set; }
        //购买成功七天后有效
        public bool IsValid { get; set; }

        //获取
        public WalletItem(RewardType type, string orderInfoId, double number)
        {
            this.OrderInfoId = orderInfoId;
            this.RewardType = type;
            this.Number = number;
            this.Time = DateTime.Now;
        }

        //使用
        public WalletItem(string orderInfoId, double number)
        {
            this.OrderInfoId = orderInfoId;
            this.Number = number;
            this.Time = DateTime.Now;
        }
    }
}
