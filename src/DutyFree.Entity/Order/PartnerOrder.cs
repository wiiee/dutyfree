namespace DutyFree.Entity.Order
{
    using Platform.Enum;
    using System.Collections.Generic;
    using System.Linq;

    //根据用户的订单系统自动生成订单
    public class PartnerOrder : BaseEntity
    {
        //Id为UserId

        public List<PartnerOrderInfo> PartnerOrderInfos { get; set; }

        //有可能有删除的订单，用于新增订单的Id
        public int TotalCount { get; set; }

        public PartnerOrder(string userId)
        {
            this.Id = userId;
            this.TotalCount = 0;
            this.PartnerOrderInfos = new List<PartnerOrderInfo>();
        }

        public PartnerOrderInfo CreatePartnerOrderInfo()
        {
            var partnerOrderInfo = new PartnerOrderInfo();
            partnerOrderInfo.Id = this.Id + "_" + TotalCount++;

            PartnerOrderInfos.Add(partnerOrderInfo);

            return partnerOrderInfo;
        }

        //获得当前活动订单
        public PartnerOrderInfo GetActive()
        {
            return PartnerOrderInfos.FirstOrDefault(o => o.Status != Status.Done);
        }
    }
}
