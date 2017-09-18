namespace DutyFree.Platform.Util
{
    using ImageProcessorCore;
    using Model;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public static class ImgUtil
    {
        public static byte[] Resize(byte[] content, int destHeight, int destWidth)
        {
            var srcStream = new MemoryStream(content);
            var output = new MemoryStream();
            var imgSource = new Image(srcStream);

            var height = Math.Min(imgSource.Height, destHeight);
            var width = Math.Min(imgSource.Width, destWidth);

            imgSource.Resize(width, height)
                .Save(output);

            return output.ToArray();
        }

        public static string GetImgUrl(string imgId)
        {
            return "api/img/" + imgId;
        }

        public static void Replace(string imgName, List<Coordinate> recs)
        {
            using (FileStream input = File.OpenRead(@"E:\DutyFree\pics\" + imgName))
            using (FileStream output = File.OpenWrite(@"E:\DutyFree\replace\" + imgName))
            {
                var items = recs.OrderBy(o => o.Y1).ToList();
                var height = items.Sum(o => (o.Y2 - o.Y1 + 1));
                Image source = new Image(input);
                Image img = new Image(source.Width, source.Height - height);

                using (PixelAccessor<Color, uint> sourcePixels = source.Lock())
                using (PixelAccessor<Color, uint> pixels = img.Lock())
                {
                    //
                    var result = new List<Color[]>();
                    int last = 0;
                    foreach(var item in items)
                    {
                        for(int i = last; i < item.Y1; i++)
                        {
                            Color[] row = new Color[img.Width];
                            for(int j = 0; j < img.Width; j++)
                            {
                                row[j] = sourcePixels[j, i];
                            }
                            result.Add(row);
                        }
                        last = item.Y2 + 1;
                    }

                    for (int i = last; i < source.Height; i++)
                    {
                        Color[] row = new Color[img.Width];
                        for (int j = 0; j < img.Width; j++)
                        {
                            row[j] = sourcePixels[j, i];
                        }
                        result.Add(row);
                    }

                    for(int i = 0; i < result.Count; i++)
                    {
                        for(int j = 0; j < img.Width; j++)
                        {
                            pixels[j, i] = result[i][j];
                        }
                    }
                }

                img.Save(output);
            }
        }
    }
}
