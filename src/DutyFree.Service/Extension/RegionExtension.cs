namespace DutyFree.Service.Extension
{
    using Context;
    using Entity.Standard;
    using Standard;

    public static class RegionExtension
    {
        private static RegionService regionService = new RegionService(ServiceContextRepository.Instance);

        public static int GetLevel(this Region region)
        {

            int level = 0;
            var next = region.ParentId;

            while (!string.IsNullOrEmpty(next))
            {
                level++;
                next = regionService.Get(next).ParentId;
            }

            return level;
        }

        public static bool HasChild(this Region region)
        {
            return regionService.Get(o => o.ParentId == region.Id).Count > 0;
        }
    }
}
