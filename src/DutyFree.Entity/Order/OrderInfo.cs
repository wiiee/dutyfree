namespace DutyFree.Entity.Order
{
    using Platform.Enum;
    using Platform.Extension;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ValueType;
    using ValueType.Order;

    public class OrderInfo : BaseEntity
    {
        //Id: [UserId]_[Index]

        //数量和价格，标明产品的状态
        public Dictionary<string, ProductInfo> ProductInfos;

        //订单收货地址
        public Address Address { get; set; }

        public double TotalPrice { get; set; }

        //父订单
        //public string ParentId { get; set; }

        //是否付款
        public bool IsPaid { get; set; }

        //快递单号
        public List<string> DeliveryIds { get; set; }

        //付款时间
        public DateTime PaidTime { get; set; }

        //订单状态
        //Initial: 初始状态
        //Pending: 已付款
        //Ongoing: 正在进行
        //Done: 订单完成
        public Status Status { get; set; }

        public OrderType OrderType { get; set; }

        //分配状态,订单一产生就立即分配
        public bool IsAssigned { get; set; }

        public OrderInfo(string id, Address address, Dictionary<string, ProductInfo> productInfos, double totalPrice)
        {
            this.Id = id;
            this.Address = address;
            this.ProductInfos = productInfos;
            this.TotalPrice = totalPrice;
            this.Created = DateTime.Now;
        }

        public int GetCount()
        {
            var result = 0;

            if (!ProductInfos.IsEmpty())
            {
                result = ProductInfos.Sum(o => o.Value.Quantity);
            }

            return result;
        }

        public int GetPendingReviewCount()
        {
            if (Status != Status.Done)
            {
                return 0;
            }

            return ProductInfos.Where(o => string.IsNullOrEmpty(o.Value.CommentId)).Count();
        }

        public List<KeyValuePair<string, string>> GetPendingReviewItems()
        {
            var result = new List<KeyValuePair<string, string>>();

            foreach(var item in ProductInfos)
            {
                if(string.IsNullOrEmpty(item.Value.CommentId))
                {
                    result.Add(new KeyValuePair<string, string>(item.Key, Id));
                }
            }

            return result;
        }

        public int GetDoneReviewCount()
        {
            if (Status != Status.Done)
            {
                return 0;
            }

            return ProductInfos.Where(o => string.IsNullOrEmpty(o.Value.CommentId)).Count();
        }

        public List<KeyValuePair<string, string>> GetDoneReviewItems()
        {
            var result = new List<KeyValuePair<string, string>>();

            foreach (var item in ProductInfos)
            {
                if (!string.IsNullOrEmpty(item.Value.CommentId))
                {
                    result.Add(new KeyValuePair<string, string>(item.Key, item.Value.CommentId.ToString()));
                }
            }

            return result;
        }

        public void AddCommentId(string productId, string commentId)
        {
            if (ProductInfos.ContainsKey(productId))
            {
                ProductInfos[productId].CommentId = commentId;
            }
        }

        public void AddPartnerOrderId(string productId, string partnerOrderInfoId, int quantity)
        {
            if(!ProductInfos.IsEmpty() && ProductInfos.ContainsKey(productId))
            {
                if(ProductInfos[productId].PartnerOrderIds == null)
                {
                    ProductInfos[productId].PartnerOrderIds = new Dictionary<string, int>();
                }

                if (ProductInfos[productId].PartnerOrderIds.ContainsKey(partnerOrderInfoId))
                {
                    ProductInfos[productId].PartnerOrderIds[partnerOrderInfoId] += quantity;
                }
                else
                {
                    ProductInfos[productId].PartnerOrderIds.Add(partnerOrderInfoId, quantity);
                }
            }
        }

        public void RemovePartnerOrderId(string productId, string partnerOrderInfoId, int quantity)
        {
            if (!ProductInfos.IsEmpty() && ProductInfos.ContainsKey(productId))
            {
                if (ProductInfos[productId].PartnerOrderIds[partnerOrderInfoId] < quantity)
                {
                    //ToDo
                    throw new Exception("");
                }
                else if(ProductInfos[productId].PartnerOrderIds[partnerOrderInfoId] == quantity)
                {
                    ProductInfos[productId].PartnerOrderIds.Remove(partnerOrderInfoId);
                }
                else
                {
                    ProductInfos[productId].PartnerOrderIds[partnerOrderInfoId] -= quantity;
                }
            }
        }

        public bool IsUnpaid()
        {
            return !IsPaid;
        }


        public bool IsOngoing()
        {
            return IsPaid && Status != Status.Done;
        }

        public bool IsDone()
        {
            return Status == Status.Done;
        }
    }
}
