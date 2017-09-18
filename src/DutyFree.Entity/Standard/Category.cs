namespace DutyFree.Entity.Standard
{
    using System.Collections.Generic;
    using ValueType;

    public class Category : BaseEntity
    {
        //Id

        public LocalField Name { get; set; }

        public string Description { get; set; }

        public string ParentId { get; set; }

        //对应的类别有哪些属性表，是否必选
        public Dictionary<string, bool> Properties { get; set; }
    }
}