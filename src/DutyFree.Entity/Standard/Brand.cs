namespace DutyFree.Entity.Standard
{
    using ValueType;

    //品牌，如Clinque,Loreal
    public class Brand : BaseEntity
    {
        //Id为英文品牌名字

        public LocalField Name { get; set; }

        public string Description { get; set; }

        public string Logo { get; set; }

        public string CountryId { get; set; }
    }
}
