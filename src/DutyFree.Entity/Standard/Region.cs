namespace DutyFree.Entity.Standard
{
    using ValueType;

    public class Region : BaseEntity
    {
        //Id

        public LocalField Name { get; set; }

        public string ParentId { get; set; }
    }
}
