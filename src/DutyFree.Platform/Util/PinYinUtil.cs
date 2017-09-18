namespace DutyFree.Platform.Util
{
    using System.Globalization;
    using System.Text.RegularExpressions;
    public static class PinYinUtil
    {
        // 汉字转化为拼音
        //public static string GetPinyin(string str)
        //{
        //    string r = string.Empty;
        //    foreach (char obj in str)
        //    {
        //        try
        //        {
        //            ChineseChar chineseChar = new ChineseChar(obj);
        //            string t = chineseChar.Pinyins[0].ToString();
        //            r += t.Substring(0, t.Length - 1);
        //        }
        //        catch
        //        {
        //            r += obj.ToString();
        //        }
        //    }
        //    return r;
        //}

        public static string DecodeEncodedNonAsciiCharacters(string value)
        {
            return Regex.Replace(
                value,
                @"\\u(?<Value>[a-zA-Z0-9]{4})",
                m => {
                    return ((char)int.Parse(m.Groups["Value"].Value, NumberStyles.HexNumber)).ToString();
                });
        }
    }
}
