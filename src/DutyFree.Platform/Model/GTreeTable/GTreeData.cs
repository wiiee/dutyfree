namespace DutyFree.Platform.Model.GTreeTable
{
    public class GTreeData
    {
        public bool hasChild { get; set; }

        public GTreeData(bool child)
        {
            this.hasChild = child;
        }
    }
}
