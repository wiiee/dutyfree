namespace DutyFree.Platform.Model.GTreeTable
{
    public class GTreeTable
    {
        public string id { get; }
        public string name { get; }
        public int level { get; }
        public string type { get; }
        public GTreeData model { get; }

        public GTreeTable(string id, string name, int level, GTreeData model, string type = "default")
        {
            this.id = id;
            this.name = name;
            this.level = level;
            this.model = model;
            this.type = type;
        }
    }
}
