namespace DutyFree.Service.Order
{
    using Entity.Order;
    using Platform.Context;
    using Platform.Util.Domain;
    using System.Collections.Generic;

    public class PartnerOrderService : BaseService<PartnerOrder>
    {
        public PartnerOrderService(IContextRepository contextRepository) : base(contextRepository) { }

        public List<PartnerOrderInfo> GetPartnerOrderInfos(List<string> partnerOrderInfoIds)
        {
            var result = new List<PartnerOrderInfo>();

            foreach (var item in partnerOrderInfoIds)
            {
                result.Add(GetPartnerOrderInfo(item));
            }

            return result;
        }

        public List<PartnerOrderInfo> GetPartnerOrderInfos()
        {
            var result = new List<PartnerOrderInfo>();
            Get().ForEach(o => result.AddRange(o.PartnerOrderInfos));

            return result;
        }

        public PartnerOrderInfo GetPartnerOrderInfo(string partnerOrderInfoId)
        {
            var userId = OrderUtil.GetUserId(partnerOrderInfoId);
            var order = base.Get(userId);
            return order.PartnerOrderInfos.Find(o => o.Id == partnerOrderInfoId);
        }

        public void UpdatePartnerOrderInfo(PartnerOrderInfo partnerOrderInfo)
        {
            var userId = OrderUtil.GetUserId(partnerOrderInfo.Id);
            var order = base.Get(userId);
            var index = order.PartnerOrderInfos.FindIndex(o => o.Id == partnerOrderInfo.Id);
            base.Update(userId, "PartnerOrderInfos." + index, partnerOrderInfo);
        }

        public PartnerOrderInfo GetActivePartnerOrderInfo(string userId)
        {
            var partnerOrder = base.Get(userId);
            if(partnerOrder == null)
            {
                partnerOrder = new PartnerOrder(userId);
                base.Create(partnerOrder);
            }

            var partnerOrderInfo = partnerOrder.GetActive();

            if(partnerOrderInfo == null)
            {
                partnerOrderInfo = partnerOrder.CreatePartnerOrderInfo();
                base.Update(partnerOrder);
            }

            return partnerOrderInfo;
        }

        public void SetFlightNo(string userId, string flightNo)
        {
            var partnerOrderInfo = GetActivePartnerOrderInfo(userId);
            partnerOrderInfo.FlightNo = flightNo;
            this.UpdatePartnerOrderInfo(partnerOrderInfo);
        }
    }
}
