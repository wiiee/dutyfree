namespace DutyFree.Entity.Order
{
    using System.Collections.Generic;
    using ValueType;
    using ValueType.Order;

    public class Order : BaseEntity
    {
        //Id为UserId

        public List<OrderInfo> OrderInfos { get; set; }

        //有可能有删除的订单，用于新增订单的Id
        public int TotalCount { get; set; }

        public Order(string userId)
        {
            this.Id = userId;
            this.TotalCount = 0;
            this.OrderInfos = new List<OrderInfo>();
        }

        public void AddOrderInfo(OrderInfo orderInfo)
        {
            if (string.IsNullOrEmpty(orderInfo.Id))
            {
                orderInfo.Id = this.Id + "_" + TotalCount++;
            }
            
            OrderInfos.Add(orderInfo);
        }

        public string AddOrderInfo(Address address, Dictionary<string, ProductInfo> productInfos, double totalPrice)
        {
            var orderInfoId = this.Id + "_" + TotalCount++;
            OrderInfos.Add(new OrderInfo(orderInfoId, address, productInfos, totalPrice));
            return orderInfoId;
        }
    }
}
