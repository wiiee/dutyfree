namespace DutyFree.Platform.Util
{
    using System.Text;
    using System.Text.RegularExpressions;
    public static class StringUtil
    {
        private static UTF8Encoding encoding = new UTF8Encoding();

        public static byte[] StrToByteArray(string str)
        {
            return encoding.GetBytes(str);
        }

        public static string ByteArrayToStr(byte[] bytes)
        {
            if(bytes == null)
            {
                return null;
            }

            return encoding.GetString(bytes);
        }

        public static string StripHtml(string input)
        {
            return Regex.Replace(input, "<.*?>", string.Empty);
        }

        public static string GetSecretPhone(string userId)
        {
            if(!string.IsNullOrEmpty(userId) && userId.Length > 4)
            {
                return "****" + userId.Substring(userId.Length - 4);
            }
            else
            {
                return userId;
            }
            
        }
    }
}
