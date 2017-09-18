namespace DutyFree.Mobile.Api
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Platform.Util;
    using Service.Media;
    using System.Collections.Generic;
    using WebCore.Base;

    [Route("api/[controller]")]
    public class ImgController : BaseController
    {
        private static ILogger _logger = LoggerUtil.CreateLogger<ImgController>();

        private ImgService _imgService;

        public ImgController(ImgService imgService)
        {
            this._imgService = imgService;
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public FileContentResult Get(string id, int height, int width)
        {
            var img = this._imgService.Get(id);

            if (img == null)
            {
                return null;
            }

            byte[] bytes;

            if (width == 0 || height == 0)
            {
                bytes = img.Content;
            }
            else
            {
                bytes = ImgUtil.Resize(img.Content, height, width);
            }

            //transform the picture's data from string to an array of bytes

            //return array of bytes as the image's data to action's response. We set the image's content mime type to image/jpeg
            return new FileContentResult(bytes, img.ContentType);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
