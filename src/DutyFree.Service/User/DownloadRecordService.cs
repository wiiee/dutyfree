namespace DutyFree.Service.User
{
    using Entity.User;
    using Platform.Context;

    public class DownloadRecordService : BaseService<DownloadRecord>
    {
        public DownloadRecordService(IContextRepository contextRepository) : base(contextRepository) { }
    }
}