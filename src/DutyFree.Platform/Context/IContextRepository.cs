namespace DutyFree.Platform.Context
{
    public interface IContextRepository
    {
        IContext GetCurrent();
    }
}
