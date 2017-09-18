namespace DutyFree.Service.Standard
{
    using Entity.Standard;
    using Microsoft.Extensions.Logging;
    using Model;
    using Platform.Context;
    using Platform.Extension;
    using Platform.Util;
    using System.Collections.Generic;
    using System.Linq;

    public class RegionService : BaseService<Region>
    {
        private static ILogger _logger = LoggerUtil.CreateLogger<RegionService>();
        public RegionService(IContextRepository contextRepository) : base(contextRepository) { }
        private int count = 0;

        public List<LinkedList> GetRegions()
        {
            var result = new List<LinkedList>();

            var topCategoryIds = GetTopCategoryIds();

            foreach (var categoryId in topCategoryIds)
            {
                var node = new LinkedList();
                node.Id = categoryId;

                BuildLinkedList(node);

                result.Add(node);
            }

            return result;
        }

        private void BuildLinkedList(LinkedList node)
        {
            count++;
            _logger.LogInformation(node.Id);
            var nexts = Get(o => o.ParentId == node.Id).Select(o => o.Id).ToList();

            if (nexts.IsEmpty())
            {
                _logger.LogInformation("quit: " + node.Id);
                return;
            }
            else
            {
                node.Nexts = new List<LinkedList>();

                foreach (var id in nexts)
                {
                    var nextNode = new LinkedList();
                    nextNode.Id = id;

                    BuildLinkedList(nextNode);

                    node.Nexts.Add(nextNode);
                }
            }
        }

        //获取某个商品的类别
        public List<string> GetCategoriesByChild(string childId)
        {
            var result = new List<string>();
            result.Add(childId);
            var category = Get(childId);

            while (!string.IsNullOrEmpty(category.ParentId))
            {
                result.Add(category.ParentId);
                category = Get(category.ParentId);
            }

            result.Reverse();

            return result;
        }

        //获取某个商品的类别
        public List<List<string>> GetCategoryChain(string childId)
        {
            var result = new List<List<string>>();
            var category = Get(childId);

            while (category != null && !string.IsNullOrEmpty(category.ParentId))
            {
                result.Add(Get(o => o.ParentId == category.ParentId).Select(o => o.Id).ToList());
                category = Get(category.ParentId);
            }

            result.Add(GetTopCategoryIds());

            result.Reverse();

            return result;
        }

        //获取所有的子目录
        public LinkedList GetCategoriesByParent(string parentId)
        {
            var linkedList = new LinkedList();
            linkedList.Id = parentId;

            BuildLinkedList(linkedList);

            return linkedList;
        }

        //获取顶级目录
        public List<string> GetTopCategoryIds()
        {
            return Get(o => string.IsNullOrEmpty(o.ParentId)).Select(o => o.Id).ToList();
        }
    }
}
