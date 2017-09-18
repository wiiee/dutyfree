namespace DutyFree.Entity.ValueType
{
    using Platform.Enum;
    using System.Collections.Generic;

    public class UnitValue
    {
        public UnitValueType Type { get; set; }

        //Range类型
        public double Min { get; set; }
        public double Max { get; set; }

        //列表类型
        public HashSet<string> Values { get; set; }
    }
}
