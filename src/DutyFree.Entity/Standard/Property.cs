namespace DutyFree.Entity.Standard
{
    using System.Collections.Generic;
    using ValueType;

    public class Property : BaseEntity
    {
        //Id

        public LocalField Name { get; set; }

        public string Description { get; set; }

        //Id为unit Id
        public List<KeyValuePair<string, UnitValue>> Units { get; set; }
    }
}
