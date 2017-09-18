namespace DutyFree.WebCore.Middleware
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using System.Threading.Tasks;

    public class UserTypeMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public UserTypeMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<UserTypeMiddleware>();
        }

        public async Task Invoke(HttpContext context)
        {
            //var partnerHost = Setting.Instance.Get("PartnerHost");

            //if (context.Request.Host.Value.ToLower().Contains(partnerHost))
            //{
            //    context.Session.SetInt32(SessionKey.USER_TYPE, (int)UserType.Partner);
            //}
            //else
            //{
            //    context.Session.SetInt32(SessionKey.USER_TYPE, (int)UserType.User);
            //}

            string headers = "";
            foreach(var header in context.Request.Headers)
            {
                headers += header.Key + ":" + header.Value + "|";
            }

            string form = "";
            
            if(context.Request.HasFormContentType)
            {
                var keys = "";
                foreach(var key in context.Request.Form.Keys)
                {
                    keys += key + ",";
                }

                var files = "";
                foreach(var file in context.Request.Form.Files)
                {
                    string fileHeaders = "";

                    foreach (var header in file.Headers)
                    {
                        fileHeaders += header.Key + ":" + header.Value + "|";
                    }

                    files += "fileName:" + file.FileName +  ", fileHeaders:" + fileHeaders;
                }



                form += "form.count:" + context.Request.Form.Count + ", keys:" + keys + ", files:" + files;
            }


            _logger.LogCritical(string.Format("Headers: {0}\nBody: {1}\nContentLength: {2}\nContentType: {3}\nForm: {4}", headers, context.Request.Body, context.Request.ContentLength, 
                context.Request.ContentType, form));

            await _next.Invoke(context);
        }
    }
}
