namespace DutyFree.Entity.Media
{
    public class Img : BaseEntity
    {
        public byte[] Content { get; set; }

        public int Length { get; set; }

        public int Height { get; set; }
        public int Width { get; set; }

        public string Name { get; set; }

        public string ContentType { get; set; }

        public Img(int length, string name, byte[] content, string contentType)
        {
            this.Length = length;
            this.Name = name;
            this.Content = content;
            this.ContentType = contentType;
        }

        public Img(int length, string name, byte[] content, string contentType, int width, int height)
        {
            this.Length = length;
            this.Name = name;
            this.Content = content;
            this.ContentType = contentType;
            this.Height = height;
            this.Width = width;
        }

        public Img() { }
    }
}
