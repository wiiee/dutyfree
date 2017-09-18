namespace DutyFree.Service.Standard
{
    using DataAccess.Database.Base;
    using DataAccess.Database.Manager;
    using Entity.Standard;
    using Microsoft.Extensions.Logging;
    using Platform.Util;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class OldRegionService
    {
        private IDatabase db;
        private static ILogger _logger = LoggerUtil.CreateLogger<OldRegionService>();

        public OldRegionService()
        {
            db = DatabaseManager.Instance.GetDatabase("freeExpress");
        }


        public void Save(List<OldRegion> entities)
        {
            try
            {
                var task = Task.Run(async () => { await db.GetMongoCollection<OldRegion>().InsertManyAsync(entities); });
                task.Wait();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return;
            }
        }

    }
}
