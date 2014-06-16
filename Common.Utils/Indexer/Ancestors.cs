#region

using System.Collections.Generic;
using System.Linq;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.ContentSearch.Utilities;
using Sitecore.Data.Items;

#endregion

namespace Projects.Common.Core.Indexer
{
    public class Ancestors : IComputedIndexField
    {
        #region IComputedIndexField Members

        public object ComputeFieldValue(IIndexable indexable)
        {
            Item item = (indexable as SitecoreIndexableItem);
            if (item != null && !item.Paths.IsMediaItem)
            {
                List<string> ancestorId = item.Axes.GetAncestors()
                                              .Select(source => IdHelper.NormalizeGuid(source.ID))
                                              .ToList();

                return ancestorId;
            }
            return null;
        }

        public string FieldName { get; set; }

        public string ReturnType { get; set; }

        #endregion
    }
}