namespace DutyFree.Platform.Util
{
    using System;
    using System.Net.Http;

    public static class HttpUtil
    {
        public static string GetString(string url)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                var response = client.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsStringAsync().Result;
                    //do something with the response here. Typically use JSON.net to deserialise it and work with it
                }
                else
                {
                    return "error";
                }
            }
        }

        public static byte[] GetBytes(string url)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                var response = client.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsByteArrayAsync().Result;
                    //do something with the response here. Typically use JSON.net to deserialise it and work with it
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
