using System.Collections.Generic;

namespace DutyFree.Entity.Promote
{
    //满减
    public class LimitDiscount : IDiscount
    {
        public List<KeyValuePair<double, double>> Count { get; set; }
    }
}
