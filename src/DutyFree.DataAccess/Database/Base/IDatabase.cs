namespace DutyFree.DataAccess.Database.Base
{
    using Entity;
    using MongoDB.Driver;

    public interface IDatabase
    {
        IDatabaseSetting GetDatabaseSetting();

        IDbCollection<T> GetCollection<T>() where T : IEntity;

        IMongoCollection<T> GetMongoCollection<T>();

        void DropCollection<T>() where T : IEntity;

        void DropCollection(string name);

        void RenameCollection(string oldName, string newName);
    }
}
