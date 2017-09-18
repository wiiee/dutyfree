namespace DutyFree.Service.Product
{
    using Entity.Order;
    using Entity.ValueType.Order;
    using Extension;
    using Order;
    using Platform.Constant;
    using Platform.Context;
    using Platform.Setting;
    using System;
    using System.Collections.Generic;

    public class PartnerProductService : BaseService<PartnerProduct>
    {
        private object _lockObj = new object();
        public PartnerProductService(IContextRepository contextRepository) : base(contextRepository) { }

        //添加到购物车
        public bool AddCart(string userId, string productId, int quantity)
        {
            lock (_lockObj)
            {
                var orderService = ServiceFactory.Instance.GetService<OrderService>();
                var partnerOrderService = ServiceFactory.Instance.GetService<PartnerOrderService>();
                var partnerOrderInfo = partnerOrderService.GetActivePartnerOrderInfo(userId);

                //用户订单Id和数量
                var orderInfos = new Dictionary<string, int>();

                var product = this.Get(productId);
                var result = product.AddCart(quantity, ref partnerOrderInfo, ref orderInfos);

                if (result)
                {
                    base.Update(product);
                    partnerOrderService.UpdatePartnerOrderInfo(partnerOrderInfo);

                    //更新源订单里面的信息
                    foreach (var item in orderInfos)
                    {
                        var orderInfo = orderService.GetOrderInfo(item.Key);
                        orderInfo.AddPartnerOrderId(productId, partnerOrderInfo.Id, item.Value);
                        orderService.UpdateOrderInfo(orderInfo);
                    }
                }

                return result;
            }
        }

        //移除购物车的商品
        public void RemoveCart(string userId, string productId, int quantity)
        {
            lock (_lockObj)
            {
                var orderService = ServiceFactory.Instance.GetService<OrderService>();
                var partnerOrderService = ServiceFactory.Instance.GetService<PartnerOrderService>();
                var partnerOrderInfo = partnerOrderService.GetActivePartnerOrderInfo(userId);

                //用户订单Id和数量
                var orderInfos = new Dictionary<string, int>();

                var product = base.Get(productId);
                product.RemoveCart(productId, quantity, ref partnerOrderInfo, ref orderInfos);

                //更新源订单里面的信息
                foreach (var item in orderInfos)
                {
                    var orderInfo = orderService.GetOrderInfo(item.Key);
                    orderInfo.RemovePartnerOrderId(productId, partnerOrderInfo.Id, item.Value);
                    orderService.UpdateOrderInfo(orderInfo);
                }

                base.Update(product);
                partnerOrderService.UpdatePartnerOrderInfo(partnerOrderInfo);
            }
        }

        //生成订单时加入进来
        public void AddOrderInfo(OrderInfo orderInfo)
        {
            lock (_lockObj)
            {
                var ticks = DateTime.Now.Ticks;
                foreach (var item in orderInfo.ProductInfos)
                {
                    var product = this.Get(item.Key);
                    if (product != null)
                    {
                        product.PartnerProductInfo.Quantity += item.Value.Quantity;
                        product.PartnerProductInfo.OrderInfos.Add(orderInfo.Id, new InfoItem(item.Value.Quantity, ticks));
                        base.Update(product);
                    }
                    else
                    {
                        base.Create(new PartnerProduct(item.Key, item.Value.Quantity, orderInfo.Id));
                    }
                }
            }
        }

        public Dictionary<string, int> GetProducts(string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                var partnerOrderService = ServiceFactory.Instance.GetService<PartnerOrderService>();
                var productService = ServiceFactory.Instance.GetService<ProductService>();
                var partnerOrderInfo = partnerOrderService.GetActivePartnerOrderInfo(userId);
                var maxPrice = OrderConstant.PRICE_MAX;

                var remaining = Math.Max(0, maxPrice - partnerOrderInfo.GetTotalPrice());

                var result = new Dictionary<string, int>();

                foreach (var item in base.Get())
                {
                    if (item.PartnerProductInfo.Quantity > 0)
                    {
                        result.Add(item.Id, Math.Min(item.PartnerProductInfo.Quantity, (int)Math.Floor(remaining / productService.Get(item.Id).Price)));
                    }
                }

                return result;
            }

            return null;
        }
    }
}