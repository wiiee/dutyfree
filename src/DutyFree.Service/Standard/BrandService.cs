namespace DutyFree.Service.Standard
{
    using Entity.Standard;
    using Platform.Context;

    public class BrandService : BaseService<Brand>
    {
        public BrandService(IContextRepository contextRepository) : base(contextRepository) { }
    }
}
