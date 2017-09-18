namespace DutyFree.DataAccess.Database.Impl.MongoDb
{
    using Base;
    using Entity;
    using MongoDB.Driver;
    using System.Threading.Tasks;
    using System;

    public class MongoDbImpl : IDatabase
    {
        private IDatabaseSetting databaseSetting;

        private MongoClient mongoClient;
        private IMongoDatabase mongoDatabase;

        private const string SEQ_INITIAL = "1";

        public MongoDbImpl(IDatabaseSetting databaseSetting)
        {
            this.databaseSetting = databaseSetting;
            //string connectionString = "mongodb://" +
            //    databaseSetting.GetUserName() + ":" +
            //    databaseSetting.GetPassword() + "@" +
            //    databaseSetting.GetAddress() + "/" +
            //    "admin";

            mongoClient = new MongoClient();
            mongoDatabase = mongoClient.GetDatabase(databaseSetting.GetDatabaseName());
        }

        public void RenameCollection(string oldName, string newName)
        {
            mongoDatabase.RenameCollection(oldName, newName);
        }

        public void DropCollection(string name)
        {
            mongoDatabase.DropCollection(name);
        }

        public IDatabaseSetting GetDatabaseSetting()
        {
            return databaseSetting;
        }

        public IDbCollection<T> GetCollection<T>()
            where T : IEntity
        {
            return new MongoDbCollection<T>(mongoDatabase);
        }

        public void DropCollection<T>()
            where T : IEntity
        {
            var task = Task.Run(async () => { await mongoDatabase.DropCollectionAsync(typeof(T).Name); });
            task.Wait();
        }

        public IMongoCollection<T> GetMongoCollection<T>()
        {
            return mongoDatabase.GetCollection<T>(typeof(T).Name);
        }
    }
}