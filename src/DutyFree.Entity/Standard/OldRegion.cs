namespace DutyFree.Entity.Standard
{
    public class OldRegion
    {
        public int Id { get; set; }

        public string name { get; set; }

        public int parentId { get; set; }

        public OldRegion(int id, string name, int parentId)
        {
            this.Id = id;
            this.name = name;
            this.parentId = parentId;
        }
    }
}
