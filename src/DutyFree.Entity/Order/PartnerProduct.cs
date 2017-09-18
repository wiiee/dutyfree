namespace DutyFree.Entity.Order
{
    using System.Collections.Generic;
    using System.Linq;
    using ValueType.Order;

    //购物列表池
    public class PartnerProduct : BaseEntity
    {
        //Id为ProductId

        public PartnerProductInfo PartnerProductInfo { get; set; }

        //首页点击购买时产生的，加入到购物车里面去
        public bool AddCart(int quantity, ref PartnerOrderInfo partnerOrderInfo, ref Dictionary<string, int> orderInfos)
        {
            //购买数量不足
            if(PartnerProductInfo.Quantity < quantity)
            {
                return false;
            }
            else
            {
                //从产品池里面减掉数量
                PartnerProductInfo.Quantity -= quantity;
                PartnerProductInfo.OrderInfos = PartnerProductInfo.OrderInfos.OrderByDescending(o => o.Value.Time).ToDictionary(o => o.Key, o => o.Value);
                //移除相应的订单来源，减掉的部分添加到partnerOrderInfo里面去
                PartnerProductInfo.AddCart(this.Id, quantity, ref partnerOrderInfo, ref orderInfos);
                return true;
            }
        }

        //取消购买，如果quantity为0则移除整个
        public void RemoveCart(string productId, int quantity, ref PartnerOrderInfo partnerOrderInfo, ref Dictionary<string, int> orderInfos)
        {
            PartnerProductInfo.RemoveCart(productId, quantity, ref partnerOrderInfo, ref orderInfos);
        }

        public PartnerProduct(string id, int quantity, string orderInfoId)
        {
            this.Id = id;
            this.PartnerProductInfo = new PartnerProductInfo(quantity, orderInfoId);
        }
    }
}
