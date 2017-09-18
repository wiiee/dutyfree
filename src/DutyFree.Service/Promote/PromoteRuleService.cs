namespace DutyFree.Service.Promote
{
    using Entity.Promote;
    using Platform.Context;

    public class PromoteRuleService : BaseService<PromoteRule>
    {
        public PromoteRuleService(IContextRepository contextRepository) : base(contextRepository) { }
    }
}
