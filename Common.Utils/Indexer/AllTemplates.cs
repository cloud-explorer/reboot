using System.Collections.Generic;
using Sitecore;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.ContentSearch.Utilities;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;

namespace Projects.Common.Core.Indexer
{
    /// <summary>
    /// based on http://mikael.com/2013/05/sitecore-7-query-items-that-inherits-a-template/
    /// </summary>
    public class AllTemplates : IComputedIndexField
    {
        public string FieldName { get; set; }

        public string ReturnType { get; set; }

        public object ComputeFieldValue(IIndexable indexable)
        {
            return GetAllTemplates(indexable as SitecoreIndexableItem);
        }

        private static List<string> GetAllTemplates(Item item)
        {
            Assert.ArgumentNotNull(item, "item");
            Assert.IsNotNull(item.Template, "Item template not found.");
            var list = new List<string> { IdHelper.NormalizeGuid(item.TemplateID).ToLower() };
            RecurseTemplates(list, item.Template);
            return list;
        }

        private static void RecurseTemplates(List<string> list, TemplateItem template)
        {
            foreach (var baseTemplateItem in template.BaseTemplates)
            {
                list.Add(IdHelper.NormalizeGuid(baseTemplateItem.ID).ToLower());
                if (baseTemplateItem.ID != TemplateIDs.StandardTemplate)
                    RecurseTemplates(list, baseTemplateItem);
            }
        }
    }
}
