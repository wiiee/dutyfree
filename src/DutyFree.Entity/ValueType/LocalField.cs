namespace DutyFree.Entity.ValueType
{
    using Platform.Extension;
    using System.Collections.Generic;
    using System.Globalization;

    //本地化
    public class LocalField
    {
        //
        public static string LOCAL_CULTURE = CultureInfo.CurrentCulture.Name;

        public string Default { get; set; }

        public Dictionary<string, string> Values { get; set; }

        public LocalField(string value)
        {
            this.Default = value;
            this.Values = new Dictionary<string, string>();
        }

        public LocalField()
        {

        }

        public string GetLocal()
        {
            if(!Values.IsEmpty() && Values.ContainsKey(LOCAL_CULTURE))
            {
                return Values[LOCAL_CULTURE];
            }

            return Default;
        }

        public string Get(string lang)
        {
            if (Values.ContainsKey(lang))
            {
                return Values[lang];
            }

            return Default;
        }


        public void Add(string culture, string value)
        {
            if (Values.IsEmpty())
            {
                Values = new Dictionary<string, string>();
            }

            if (!Values.ContainsKey(culture))
            {
                Values.Add(culture, value);
            }
        }
    }
}
