namespace DutyFree.Entity.ValueType.Order
{
    using Entity.Order;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class PartnerProductInfo
    {
        //数量
        public int Quantity { get; set; }

        //来源于哪个订单，每个订单几个产品，什么时候创建的
        public Dictionary<string, InfoItem> OrderInfos { get; set; }

        public PartnerProductInfo(int quantity, Dictionary<string, InfoItem> orderInfos)
        {
            this.Quantity = quantity;
            this.OrderInfos = orderInfos;
        }

        public PartnerProductInfo(int quantity, string orderInfoId)
        {
            this.Quantity = quantity;
            this.OrderInfos = new Dictionary<string, InfoItem>();
            this.OrderInfos.Add(orderInfoId, new InfoItem(quantity, DateTime.Now.Ticks));
        }

        //添加数量和来源，时间只对ProductPool里面有用
        private void AddOrderInfoId(string orderInfoId, int quantity, long ticks)
        {
            if (OrderInfos.ContainsKey(orderInfoId))
            {
                OrderInfos[orderInfoId].Quantity += quantity;
            }
            else
            {
                OrderInfos.Add(orderInfoId, new InfoItem(quantity, ticks));
            }
        }

        //从ProductPool里面移除到用户订单里面
        public void AddCart(string productId, int quantity, ref PartnerOrderInfo partnerOrderInfo, ref Dictionary<string, int> userOrderInfos)
        {
            //添加到用户订单里面去
            if (partnerOrderInfo.PartnerProductInfos.ContainsKey(productId))
            {
                partnerOrderInfo.PartnerProductInfos[productId].Quantity += quantity;
            }
            else
            {
                partnerOrderInfo.PartnerProductInfos.Add(productId, new PartnerProductInfo(quantity, new Dictionary<string, InfoItem>()));
            }

            var remaining = quantity;
            var keys = new List<string>(OrderInfos.Keys);
            foreach (var key in keys)
            {
                var infoItem = OrderInfos[key];
                if (infoItem.Quantity > remaining)
                {
                    infoItem.Quantity -= remaining;
                    partnerOrderInfo.PartnerProductInfos[productId].AddOrderInfoId(key, remaining, infoItem.Time);
                    userOrderInfos.Add(key, remaining);
                    break;
                }
                else if (infoItem.Quantity == remaining)
                {
                    OrderInfos.Remove(key);
                    partnerOrderInfo.PartnerProductInfos[productId].AddOrderInfoId(key, remaining, infoItem.Time);
                    userOrderInfos.Add(key, remaining);
                    break;
                }
                else
                {
                    OrderInfos.Remove(key);
                    remaining = quantity - infoItem.Quantity;
                    partnerOrderInfo.PartnerProductInfos[productId].AddOrderInfoId(key, infoItem.Quantity, infoItem.Time);
                    userOrderInfos.Add(key, infoItem.Quantity);
                }
            }
        }

        //从用户订单里面释放到ProductPool
        public void RemoveCart(string productId, int quantity, ref PartnerOrderInfo partnerOrderInfo, ref Dictionary<string, int> userOrderInfos)
        {
            var partnerProductInfo = partnerOrderInfo.PartnerProductInfos[productId];

            if (quantity < 0 || quantity > partnerProductInfo.Quantity)
            {
                throw new Exception("quantity have problem");
            }

            //移除
            if (quantity == 0)
            {
                Quantity += partnerProductInfo.Quantity;

                foreach (var item in partnerProductInfo.OrderInfos)
                {
                    AddOrderInfoId(item.Key, item.Value.Quantity, item.Value.Time);
                    userOrderInfos.Add(item.Key, item.Value.Quantity);
                }

                //带货订单移除该产品
                partnerOrderInfo.PartnerProductInfos.Remove(productId);
            }
            else
            {
                Quantity += quantity;
                var remaining = quantity;
                var keys = new List<string>(partnerProductInfo.OrderInfos.Keys);
                partnerProductInfo.OrderInfos = partnerProductInfo.OrderInfos.OrderBy(o => o.Value.Time).ToDictionary(o => o.Key, o => o.Value);

                foreach (var key in keys)
                {
                    var infoItem = partnerProductInfo.OrderInfos[key];
                    if (infoItem.Quantity > remaining)
                    {
                        infoItem.Quantity -= remaining;
                        AddOrderInfoId(key, remaining, infoItem.Time);
                        userOrderInfos.Add(key, remaining);
                        break;
                    }
                    else if (infoItem.Quantity == remaining)
                    {
                        partnerProductInfo.OrderInfos.Remove(key);
                        AddOrderInfoId(key, remaining, infoItem.Time);
                        userOrderInfos.Add(key, remaining);
                        break;
                    }
                    else
                    {
                        partnerProductInfo.OrderInfos.Remove(key);
                        remaining = quantity - OrderInfos[key].Quantity;
                        AddOrderInfoId(key, OrderInfos[key].Quantity, infoItem.Time);
                        userOrderInfos.Add(key, OrderInfos[key].Quantity);
                    }
                }

                //带货订单减去相应的数量
                partnerProductInfo.Quantity -= quantity;
            }
        }
    }
}
