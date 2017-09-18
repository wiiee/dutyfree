namespace DutyFree.Service.Standard
{
    using Entity.Standard;
    using Platform.Context;

    public class AirportService : BaseService<Airport>
    {
        public AirportService(IContextRepository contextRepository) : base(contextRepository) { }
    }
}
