namespace DutyFree.Entity.Promote
{
    using System.Collections.Generic;
    using ValueType;

    public class PromoteRule : BaseEntity
    {
        //Id

        public string Description { get; set; }

        //商品的类型
        public HashSet<string> CategoryIds { get; set; }
        //指定商品
        public HashSet<string> ProductIds { get; set; }
        //配置lambda表达式
        public string Filter { get; set; }

        //活动起始结束日期
        public  DateRange DateRange { get; set; }

        public IDiscount Discount { get; set; }

        //是否和其它优惠共享
        public bool IsShared { get; set; }

        //不能和其它优惠共享
        public HashSet<string> MuteRuleIds { get; set; }
    }
}
