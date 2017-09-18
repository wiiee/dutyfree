namespace DutyFree.Entity.ValueType.Order
{
    using System.Collections.Generic;

    public class InfoItem
    {
        public int Quantity { get; set; }
        public long Time { get; set; }

        //分配给了哪几个订单，每个订单几个
        public Dictionary<string, int> PartnerOrderIds { get; set; }

        public InfoItem(int quantity, long time)
        {
            this.Quantity = quantity;
            this.Time = time;
        }
    }
}
