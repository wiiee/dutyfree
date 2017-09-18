namespace DutyFree.Entity.Media
{
    using Platform.Model;
    using System.Collections.Generic;

    public class ImgKl : BaseEntity
    {
        //Id 图片Id

        public List<Coordinate> Recs { get; set; }

        public ImgKl(string id)
        {
            this.Id = id;
            this.Recs = new List<Coordinate>();
        }
    }
}
