using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Projects.Models.Glass.Common.FieldSections;
using Projects.Reboot.DataMigration.Utils;
using Sitecore.Buckets.Managers;
using Sitecore.Data;
using Sitecore.Data.IDTables;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Events;
using Sitecore.Buckets.Extensions;
using Sitecore.Links;
using Glass.Mapper.Sc;

namespace Projects.Reboot.DataMigration.EventHandlers
{
    public class ImportHandler
    {
        protected void OnItemDeleted(object sender, System.EventArgs args)
        {
            if (args != null)
            {
                Item item = Event.ExtractParameter(args, 0) as Item;
                Assert.IsNotNull(item, "No item in parameters");
                ID parentID = item.ParentID;
                if (BucketManager.IsItemContainedWithinBucket(item))
                {
                    parentID = item.GetParentBucketItemOrParent().ParentID;
                }
                //IHasExternalId i = item.GlassCast<IHasExternalId>();
                string extId = item[IHasExternalIdConstants.ExternalIdFieldId];
                if (!string.IsNullOrEmpty(extId))
                {
                    string prefix = item.TemplateID.Guid.GetMatchingPrefix();
                    IDTable.RemoveID(prefix, item.ID);
                }
                else if (parentID.Equals(RebootConstants.ImageRootID))
                {
                    IDTable.RemoveID(RebootConstants.ImageItemPrefix, item.ID);
                }
            }
        }

        protected void OnItemSaved(object sender, System.EventArgs args)
        {
            if ((args != null))
            {
                Item item = Event.ExtractParameter(args, 0) as Item;
                Assert.IsNotNull(item, "No item in parameters");
                ID parentID = item.ParentID;
                if (BucketManager.IsItemContainedWithinBucket(item))
                {
                    parentID = item.GetParentBucketItemOrParent().ParentID;
                }
                IHasExternalId i = item.GlassCast<IHasExternalId>();
                if (i != null && !string.IsNullOrEmpty(i.ExternalId))
                {
                    string prefix = i.TemplateId.GetMatchingPrefix();
                    if (IDTable.GetID(prefix, i.ExternalId) == null)
                    {
                        IDTable.Add(prefix, i.ExternalId , item.ID, parentID);
                    }
                }
                else if (parentID.Equals(RebootConstants.ImageRootID))
                {
                    string prefix = RebootConstants.ImageItemPrefix;
                    if (IDTable.GetID(prefix, item.Name) == null)
                    {
                        IDTable.Add(prefix, item.Name, item.ID, parentID);
                    }
                }
            }
        }
    }
}
