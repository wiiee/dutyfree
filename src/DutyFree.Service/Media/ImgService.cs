namespace DutyFree.Service.Media
{
    using Entity.Media;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using Platform.Context;
    using System;
    using System.IO;
    using System.Net.Http.Headers;

    public class ImgService : BaseService<Img>
    {
        public ImgService(IContextRepository contextRepository) : base(contextRepository) { }


        public string SaveImg(IFormFile file)
        {
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

                    return Create(img);
                }
            }
            catch (Exception ex)
            {
                GetLogger().LogError(ex.Message);
            }

            return null;
        }
    }
}
