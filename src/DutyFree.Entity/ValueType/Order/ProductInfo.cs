namespace DutyFree.Entity.ValueType.Order
{
    using System.Collections.Generic;

    public class ProductInfo
    {
        //数量
        public int Quantity { get; set; }

        //点评Id
        public string CommentId { get; set; }

        //产品价格
        public double Price { get; set; }

        //在哪几个订单里面，每个订单多少个物品
        public Dictionary<string, int> PartnerOrderIds { get; set; }

        public ProductInfo(int quantity, double price)
        {
            this.Quantity = quantity;
            this.Price = price;
            this.PartnerOrderIds = new Dictionary<string, int>();
        }

        public void AddPartnerOrderId(string partnerOrderId, int quantity)
        {
            if (PartnerOrderIds.ContainsKey(partnerOrderId))
            {
                PartnerOrderIds[partnerOrderId] += quantity;
            }
            else
            {
                PartnerOrderIds.Add(partnerOrderId, quantity);
            }
        }
    }
}
