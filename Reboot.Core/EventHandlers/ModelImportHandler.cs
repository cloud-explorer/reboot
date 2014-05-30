using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glass.Mapper.Sc;
using Projects.Models.Glass.Common.FieldSections;
using Sitecore.Buckets.Extensions;
using Sitecore.Buckets.Managers;
using Sitecore.Data;
using Sitecore.Data.IDTables;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Events;

namespace Projects.Reboot.Core.EventHandlers
{
    class ModelImportHandler
    {
        protected void OnItemDeleted(object sender, System.EventArgs args)
        {
            if (args != null)
            {
                Item item = Event.ExtractParameter(args, 0) as Item;
                Assert.IsNotNull(item, "No item in parameters");
                if (!item.TemplateID.Equals(RebootConstants.ModelTemplateId)) return;
                string prefix = item.TemplateID.ToString();
                string key = item.Name;
                IDTable.RemoveKey(prefix, key);
            }
        }

        protected void OnItemSaved(object sender, System.EventArgs args)
        {
            if ((args != null))
            {
                Item item = Event.ExtractParameter(args, 0) as Item;
                Assert.IsNotNull(item, "No item in parameters");
                if (!item.TemplateID.Equals(RebootConstants.ModelTemplateId)) return;
                ID parentID = item.ParentID;
                if (BucketManager.IsItemContainedWithinBucket(item))
                {
                    parentID = item.GetParentBucketItemOrParent().ParentID;
                }
                string prefix = item.TemplateID.ToString();
                string key = item.Name;
                IDTableEntry idTableEntry = IDTable.GetID(prefix, key);
                if(idTableEntry == null) IDTable.Add(prefix, key, item.ID, parentID);
            }
        }
    }
}
