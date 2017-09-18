namespace DutyFree.Service.Order
{
    using Entity.Order;
    using Entity.ValueType;
    using Entity.ValueType.Order;
    using Platform.Constant;
    using Platform.Context;
    using Platform.Enum;
    using Platform.Extension;
    using Platform.Util;
    using Platform.Util.Domain;
    using Product;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using User;

    public class OrderService : BaseService<Order>
    {
        public OrderService(IContextRepository contextRepository) : base(contextRepository) { }

        public void UpdateStatus(string orderId, Status status)
        {
            var userId = OrderUtil.GetUserId(orderId);
            var order = base.Get(userId);
            var item = order.OrderInfos.Find(o => o.Id == orderId);
            var index = order.OrderInfos.IndexOf(item);
            base.Update(userId, "OrderInfos." + index + ".Status", status);
        }

        public Status GetStatus(string orderId)
        {
            var order = base.Get(OrderUtil.GetUserId(orderId));
            var item = order.OrderInfos.Find(o => o.Id == orderId);
            return item.Status;
        }

        public List<OrderInfo> GetOrderInfos(List<string> orderInfoIds)
        {
            var result = new List<OrderInfo>();

            foreach(var item in orderInfoIds)
            {
                result.Add(GetOrderInfo(item));
            }

            return result;
        }

        public List<OrderInfo> GetOrderInfos()
        {
            var result = new List<OrderInfo>();

            Get().ForEach(o => result.AddRange(o.OrderInfos));

            return result;
        }

        public OrderInfo GetOrderInfo(string orderInfoId)
        {
            var userId = OrderUtil.GetUserId(orderInfoId);
            var order = base.Get(userId);
            return order.OrderInfos.Find(o => o.Id == orderInfoId);
        }

        public void UpdateOrderInfo(OrderInfo orderInfo)
        {
            var userId = OrderUtil.GetUserId(orderInfo.Id);
            var order = base.Get(userId);
            var index = order.OrderInfos.FindIndex(o => o.Id == orderInfo.Id);
            base.Update(userId, "OrderInfos." + index, orderInfo);
        }

        public string CreateOrderInfo(string userId, Address address = null)
        {
            var preferenceService = ServiceFactory.Instance.GetService<PreferenceService>();
            var productService = ServiceFactory.Instance.GetService<ProductService>();
            var partnerOrderService = ServiceFactory.Instance.GetService<PartnerOrderService>();
            var user = ServiceFactory.Instance.GetService<UserService>().Get(userId);

            var preference = preferenceService.Get(userId);

            if (preference == null)
            {
                return string.Empty;
            }

            if (preference.Carts.IsEmpty())
            {
                return string.Empty;
            }

            var order = base.Get(userId);

            if (order == null)
            {
                order = new Order(userId);
                base.Create(order);
            }

            var productInfos = new Dictionary<string, ProductInfo>();

            foreach (var item in preference.Carts)
            {
                if (item.Value.IsSelected)
                {
                    var product = productService.Get(item.Key);
                    var price = product.Price;
                    price = Math.Ceiling(price);
                    productInfos.Add(item.Key, new ProductInfo(item.Value.Quantity, price));
                }
            }

            address = address == null ? user.Addresses.FirstOrDefault() : address;

            string orderInfoId = order.AddOrderInfo(address, productInfos, productInfos.Sum(o => o.Value.Quantity * o.Value.Price));
            base.Update(order);

            preference.Carts = preference.Carts.Where(o => !o.Value.IsSelected).ToDictionary(g => g.Key, g => g.Value);
            
            preferenceService.Update(preference);

            return orderInfoId;
        }

        //付款
        public void PayOrderInfo(string orderInfoId)
        {
            var partnerOrderService = ServiceFactory.Instance.GetService<PartnerOrderService>();
            var orderInfo = GetOrderInfo(orderInfoId);

            orderInfo.PaidTime = DateTime.Now;
            orderInfo.IsPaid = true;
            orderInfo.Status = Status.Pending;

            UpdateOrderInfo(orderInfo);

            //奖励推荐人
            RewardRecommendId(orderInfoId);

            //创建分配订单,直邮的不分配
            if(orderInfo.OrderType == OrderType.Normal)
            {
                ServiceFactory.Instance.GetService<PartnerProductService>().AddOrderInfo(orderInfo);
            }
        }

        private void RewardRecommendId(string orderInfoId)
        {
            var userService = ServiceFactory.Instance.GetService<UserService>();
            var preferenceService = ServiceFactory.Instance.GetService<PreferenceService>();

            var userId = OrderUtil.GetUserId(orderInfoId);
            var order = base.Get(userId);
            var orderInfo = GetOrderInfo(orderInfoId);

            //第一单，获取5%的奖励
            if (order.OrderInfos.Count == 1)
            {
                var rewardUserId = userService.Get(userId).RecommendUserId;

                if (!string.IsNullOrEmpty(rewardUserId))
                {
                    int reward = (int)Math.Ceiling(orderInfo.TotalPrice * 0.05);
                    preferenceService.AddReward(rewardUserId, RewardType.FirstBuy, orderInfoId, reward);
                    preferenceService.AddMessage(rewardUserId, new Message(string.Format("{0}购买了{1}元的商品，您获得奖励{1}", StringUtil.GetSecretPhone(userId), orderInfo.TotalPrice, reward), false, InternalInfo.SYSTEM_ID, DateTime.Now));
                }
            }
        }

        public void UpdateCommentId(string orderInfoId, string productId, string commentId)
        {
            var orderInfo = this.GetOrderInfo(orderInfoId);
            orderInfo.AddCommentId(productId, commentId);

            UpdateOrderInfo(orderInfo);
        }

        public void DeleteOrderInfo(string orderInfoId)
        {
            var userId = OrderUtil.GetUserId(orderInfoId);
            var order = base.Get(userId);

            if(order != null && !order.OrderInfos.IsEmpty())
            {
                //只能删除没有付款的订单
                var index = order.OrderInfos.FindIndex(o => o.Id == orderInfoId);
                if(index != -1)
                {
                    if (!order.OrderInfos[index].IsPaid)
                    {
                        order.OrderInfos.RemoveAt(index);
                        base.Update(order);
                    }
                }
            }
        }
    }
}
