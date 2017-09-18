namespace DutyFree.DataAccess.Database.Base
{
    public interface IDatabaseFactory
    {
        IDatabase CreateDatabase(IDatabaseSetting databaseSetting);
    }
}
