namespace DutyFree.Service.Standard
{
    using Entity.Standard;
    using Platform.Context;

    public class UnitService : BaseService<Unit>
    {
        public UnitService(IContextRepository contextRepository) : base(contextRepository) { }
    }
}
