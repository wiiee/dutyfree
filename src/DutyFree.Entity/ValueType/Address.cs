namespace DutyFree.Entity.ValueType
{
    using System.Collections.Generic;
    using System.Text;

    public class Address
    {
        //区域
        public List<RegionItem> Regions { get; set; }

        //具体地址
        public string Text { get; set; }

        public string Name { get; set; }
        public string Phone { get; set; }

        public bool IsDefault { get; set; }

        public string GetAddress()
        {
            StringBuilder sb = new StringBuilder();

            foreach(var item in Regions)
            {
                sb.Append(item.Name + " ");
            }

            sb.Append(Text);

            return sb.ToString();
        }

        public string GetRegion()
        {
            StringBuilder sb = new StringBuilder();

            foreach (var item in Regions)
            {
                sb.Append(item.Name + " ");
            }

            return sb.ToString().Trim();
        }
    }
}
