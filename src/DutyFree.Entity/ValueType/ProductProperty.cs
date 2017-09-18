namespace DutyFree.Entity.ValueType
{
    using System.Collections.Generic;

    public class ProductProperty
    {
        public string Id { get; set; }

        //Property单位和值
        public List<KeyValuePair<string, string>> Values { get; set; }
    }
}