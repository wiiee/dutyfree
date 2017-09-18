﻿namespace DutyFree.Admin.Api
{
    using WebCore.Base;
    using Entity.Media;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Microsoft.Net.Http.Headers;
    using Platform.Model.BootstrapFileInput;
    using Platform.Util;
    using Service.Media;
    using System;
    using System.Collections.Generic;
    using System.IO;

    [Route("api/[controller]")]
    public class ImgController : BaseController
    {
        private static ILogger _logger = LoggerUtil.CreateLogger<ImgController>();

        private ImgService _imgService;

        public ImgController(ImgService imgService)
        {
            this._imgService = imgService;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public FileContentResult Get(string id, int height, int width)
        {
            var img = _imgService.Get(id);

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
                //bytes = ImgUtil.GetThumbnail(img.Content, height, width);
                bytes = img.Content;
            }

            //transform the picture's data from string to an array of bytes

            //return array of bytes as the image's data to action's response. We set the image's content mime type to image/jpeg
            return new FileContentResult(bytes, img.ContentType);
        }

        [Route("Upload")]
        [HttpPost]
        public JsonResult Upload(IFormFile file_data)
        {
            var data = SaveImg(file_data);
            return Json(new
            {
                initialPreviewConfig = data.initialPreviewConfigs,
                initialPreviews = data.initialPreviews
            });
        }

        [Route("Remove")]
        [HttpPost]
        public JsonResult Remove(string key, string feedbackId)
        {
            try
            {
                _imgService.Delete(key);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return Json(new { });
        }

        [Route("Uploads")]
        [HttpPost]
        public JsonResult Uploads(ICollection<IFormFile> file_data)
        {
            var data = SaveImgs(file_data);
            return Json(new
            {
                initialPreviewConfig = data.initialPreviewConfigs,
                initialPreview = data.initialPreviews
            });
        }

        [HttpPut]
        public void Put(IFormFile file)
        {
            SaveImg(file);
        }

        private SendingData SaveImgs(ICollection<IFormFile> files)
        {
            var result = new SendingData(new List<InitialPreview>(), new List<InitialPreviewConfig>());

            try
            {
                foreach (var file in files)
                {
                    var sendingData = SaveImg(file);
                    result.initialPreviewConfigs.AddRange(sendingData.initialPreviewConfigs);
                    result.initialPreviews.AddRange(sendingData.initialPreviews);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return result;
        }

        private SendingData SaveImg(IFormFile file)
        {
            var result = new SendingData(new List<InitialPreview>(), new List<InitialPreviewConfig>());

            try
            {
                if (file != null && file.Length > 0)
                {
                    var img = new Img();
                    img.Length = (int)file.Length;
                    //get the file's name
                    var parsedContentDisposition = ContentDispositionHeaderValue.Parse(file.ContentDisposition);
                    img.Name = parsedContentDisposition.FileName.Trim('"');
                    img.ContentType = file.ContentType;

                    //get the bytes from the content stream of the file
                    byte[] bytes = new byte[file.Length];
                    using (BinaryReader theReader = new BinaryReader(file.OpenReadStream()))
                    {
                        bytes = theReader.ReadBytes((int)file.Length);
                    }

                    //convert the bytes of image data to a string using the Base64 encoding
                    img.Content = bytes;

                    var imgId = _imgService.Create(img);

                    result.initialPreviewConfigs.Add(new InitialPreviewConfig(img.Name, "120px", "/api/img/remove", imgId, null));
                    result.initialPreviews.Add(new InitialPreview(imgId, "file-preview-image", img.Name, img.Name).GetPreview());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return result;
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            //_imgService.Delete(id);
        }
    }
}
