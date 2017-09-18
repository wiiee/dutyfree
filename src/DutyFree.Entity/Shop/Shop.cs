namespace DutyFree.Entity.Shop
{
    using System.Collections.Generic;

    //机场免税店
    public class Shop : BaseEntity
    {
        //Id为机场代码

        public List<KeyValuePair<string, string>> Products { get; set; }
    }
}