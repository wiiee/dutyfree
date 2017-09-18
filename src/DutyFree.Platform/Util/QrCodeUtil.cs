namespace DutyFree.Platform.Util
{
    using ImageProcessorCore;
    using System.IO;
    using ZXing;
    using ZXing.Common;
    using ZXing.QrCode;

    public static class QrCodeUtil
    {
        public static byte[] CreateQr(string content)
        {
            var writer = new QRCodeWriter();
            var matrix = writer.encode(content, BarcodeFormat.QR_CODE, 200, 200);

            int width = matrix.Width;
            int height = matrix.Height;

            Image img = new Image(width, height);
            using (PixelAccessor<Color, uint> pixels = img.Lock())
            {
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        pixels[y, x] = matrix[y, x] ? Color.Black : Color.White;
                    }
                }
            }

            using(var output = new MemoryStream())
            {
                img.Save(output);
                return output.ToArray();
            }
        }

        public static string ReadQr(string filePath)
        {
            using (var input = File.OpenRead(filePath))
            using (var output = new MemoryStream())
            {
                Image img = new Image(input);
                img.Save(output);
                int height = img.Height;
                int width = img.Width;

                LuminanceSource source = new RGBLuminanceSource(output.ToArray(), width, height);
                var binarizer = new HybridBinarizer(source);
                BinaryBitmap bitMap = new BinaryBitmap(binarizer);
                var reader = new QRCodeReader();
                var result = reader.decode(bitMap);

                if(result != null)
                {
                    return result.Text;
                }

                return "";
            }
        }
    }
}
