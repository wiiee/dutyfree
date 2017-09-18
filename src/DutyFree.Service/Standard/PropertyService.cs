namespace DutyFree.Service.Standard
{
    using Entity.Standard;
    using Platform.Context;

    public class PropertyService : BaseService<Property>
    {
        public PropertyService(IContextRepository contextRepository) : base(contextRepository) { }
    }
}
