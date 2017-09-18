namespace DutyFree.Mobile.Controllers
{
    using Entity.Standard;
    using Entity.User;
    using Entity.ValueType;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;
    using Platform.Constant;
    using Platform.Enum;
    using Platform.Extension;
    using Platform.Setting;
    using Platform.Util;
    using Service.Media;
    using Service.Model;
    using Service.Order;
    using Service.Product;
    using Service.Standard;
    using Service.User;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using WebCore.Base;
    using WebCore.Constant;
    using WebCore.Extension;

    public class HomeController : BaseController
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IHostingEnvironment _env;

        private DownloadRecordService _downloadRecordService;
        private PreferenceService _preferenceService;
        private ProductService _productService;
        private ImgService _imgService;
        private RegionService _regionService;
        private OldRegionService _oldRegionService;

        public HomeController(IMemoryCache memoryCache, IHostingEnvironment env, DownloadRecordService downloadRecordService, 
            PreferenceService preferenceService, ProductService productService, ImgService imgService, RegionService regionService,
            OldRegionService oldRegionService)
        {
            this._memoryCache = memoryCache;
            this._env = env;
            this._downloadRecordService = downloadRecordService;
            this._preferenceService = preferenceService;
            this._preferenceService = preferenceService;
            this._imgService = imgService;
            this._productService = productService;
            this._regionService = regionService;
            this._oldRegionService = oldRegionService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public string RefreshRegion()
        {
            var regionIds = _regionService.GetTopCategoryIds();
            var regions = _regionService.Get();

            var allIds = new Dictionary<int, List<string>>();
            var index = 0;

            foreach(var regionId in regionIds)
            {
                var node = _regionService.GetCategoriesByParent(regionId);
                buildNode(node, allIds, 0);
            }

            var mapping = new Dictionary<string, int>();

            mapping.Add("China", 0);

            foreach(var entry in allIds)
            {
                foreach(var id in entry.Value)
                {
                    mapping.Add(id, ++index);
                }
            }

            var oldRegions = new List<OldRegion>();

            foreach(var region in regions)
            {
                oldRegions.Add(new OldRegion(mapping[region.Id], region.Name.Default, region.ParentId == null ? -1 : mapping[region.ParentId]));
            }

            var ids = oldRegions.Select(o => o.Id).ToList();
            var dIds = ids.Distinct();

            _oldRegionService.Save(oldRegions);

            return "Refresh regions successfully!";
        }

        private void buildNode(LinkedList node, Dictionary<int, List<string>> allIds, int level)
        {
            if (!node.Nexts.IsEmpty())
            {
                foreach(var pNode in node.Nexts)
                {
                    if (!allIds.ContainsKey(level))
                    {
                        allIds[level] = new List<string>();
                    }

                    allIds[level].Add(pNode.Id);

                    buildNode(pNode, allIds, level + 1);
                }
            }
        }

        private void buildOldRegions(List<OldRegion> oldRegions, LinkedList node, int parentId)
        {
            if (!node.Nexts.IsEmpty())
            {
                var pId = 0;
                foreach (var pnode in node.Nexts)
                {
                    pId++;
                    var region = _regionService.Get(pnode.Id);
                    oldRegions.Add(new OldRegion(pId, region.Name.Default, parentId));

                    buildOldRegions(oldRegions, pnode, pId);
                }
            }
        }


        public string RemoveDistinct()
        {
            var products = _productService.Get();
            foreach(var product in products)
            {
                var imgIds = product.DescriptionImgIds;
                var imgs = _imgService.GetByIds(imgIds);
                imgs = imgs.GroupBy(o => o.Content, (key, group) => group.First()).ToList();

                if(imgs.Count < imgIds.Count)
                {
                    product.DescriptionImgIds = imgs.Select(o => o.Id).ToList();
                    var dupIds = imgIds.Except(product.DescriptionImgIds).ToList();
                    dupIds.ForEach(o => _imgService.Delete(o));
                }
            }

            return "done";
        }

        public IActionResult Download(string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                var preference = _preferenceService.Get(userId);

                if(preference != null)
                {
                    var ip = HttpContext.GetRemoteIp();
                    var record = _downloadRecordService.Get(userId);

                    if (record == null)
                    {
                        record = new DownloadRecord(userId, ip);
                        _downloadRecordService.Create(record);
                    }
                    else
                    {
                        record.AddRecord(ip);
                    }

                    preference.AddMessage(new Message(ip + "扫描了你的二维码", false, InternalInfo.SYSTEM_ID, DateTime.Now));
                    _preferenceService.Update(preference);
                }
            }

            return View();
        }

        public FileResult DownloadApk()
        {
            var filepath = _env.WebRootPath + @"\apk\yimeiyiyou.apk";
            byte[] fileBytes = System.IO.File.ReadAllBytes(filepath);

            this.Response.Headers.Append("Content-Disposition", "attachment; filename=\"yimeiyiyou.apk\"");
            this.Response.Headers.Append("Content-Length", fileBytes.Length.ToString());

            return File(fileBytes, "application/vnd.android.package-archive");
        }

        public IActionResult ToUser()
        {
            return Redirect("~/Category");
        }

        public IActionResult Partner()
        {
            UpdateCache("Home.Partner");

            var trustUserIds = Setting.Instance.Get("TrustUserIds").Split(',');
            ViewData["IsTrustUser"] = trustUserIds.Contains(this.GetUserId());
            ViewData["Numbers"] = GetNumbers("Home.Partner");
            return View();
        }

        public IActionResult PartnerItems(string flightNo)
        {
            return View();
        }

        private void UpdateCache(string name)
        {
            var key = CacheKey.PAGE + "." + name;
            var ids = _memoryCache.Get<Dictionary<string, DateTime>>(key);

            if(ids == null)
            {
                ids = new Dictionary<string, DateTime>();
            }

            ids = ids.Where(o => (DateTime.Now - o.Value).Minutes < 30).ToDictionary(o => o.Key, o => o.Value);

            var id = HttpContext.Session.Id;

            if (ids.ContainsKey(id))
            {
                ids[id] = DateTime.Now;
            }
            else
            {
                ids.Add(id, DateTime.Now);
            }

            _memoryCache.Set(key, ids);
        }

        private int GetNumbers(string name)
        {
            var result = 100;

            var key = CacheKey.PAGE + "." + name;

            var ips = _memoryCache.Get<Dictionary<string, DateTime>>(key);

            if (!ips.IsEmpty())
            {
                result += ips.Count;
            }

            return result;
        }
    }
}
