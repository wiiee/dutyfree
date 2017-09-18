namespace DutyFree.Service.Media
{
    using Entity.Media;
    using Platform.Context;

    public class ImgKlService : BaseService<ImgKl>
    {
        public ImgKlService(IContextRepository contextRepository) : base(contextRepository) { }
    }
}
