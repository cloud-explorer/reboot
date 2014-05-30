#region

using System;
using System.Reflection;
using Glass.Mapper.Sc;
using Glass.Mapper.Sc.Configuration.Attributes;
using Projects.Common.Utils;
using Projects.Models.Glass.Common.FieldSections;
using Projects.Models.Glass.Reboot.Items;
using Sitecore.Data;
using Sitecore.Data.IDTables;

#endregion

namespace Projects.Reboot.DataMigration.Utils
{
    internal static class IDTableExtesions
    {
        #region Class Methods

        internal static T GetItemFromKey<T>(this T obj, string key) where T : class, IHasExternalId
        {
            ID id = GetItemIdFromIDTableEntry(obj, key);
            if (id == ID.Null) return null;
            ISitecoreService service = new SitecoreService("master");
            return service.GetItem<T>(id.Guid);
        }

        internal static ID GetItemIdFromIDTableEntry(string prefix, string key)
        {
            IDTableEntry idTableEntry = IDTable.GetID(prefix, key);
            if (idTableEntry == null) return ID.Null;
            return idTableEntry.ID;
        }

        internal static ID GetItemIdFromIDTableEntry<T>(this T obj) where T : class, IHasExternalId
        {
            if (obj == null) return ID.Null;
            string key = obj.ExternalId;
            return GetItemIdFromIDTableEntry(obj, key);
        }

        internal static ID GetItemIdFromIDTableEntry<T>(this T obj, string key) where T : class, IHasExternalId
        {
            if (obj == null) return ID.Null;
            string prefix = obj.GetType().ToString();
            IDTableEntry idTableEntry = IDTable.GetID(prefix, key);
            if (idTableEntry == null) return ID.Null;
            return idTableEntry.ID;
        }

        internal static string GetMatchingPrefix(this Guid templateID)
        {
            Assembly assembly = typeof (IMovie).Assembly;
            Type typeWithAttributeValue = assembly.GetTypeWithAttributeValue<SitecoreTypeAttribute>(attribute =>
                                                                                                    attribute.TemplateId ==
                                                                                                    templateID.ToString());
            return typeWithAttributeValue.ToString();
        }

        internal static bool HasIDTableEntry<T>(this T obj) where T : class, IHasExternalId
        {
            return GetItemIdFromIDTableEntry(obj) != ID.Null;
        }

        internal static bool HasIDTableEntry<T>(string key) where T : class, IHasExternalId
        {
            string prefix = typeof (T).ToString();
            return HasIDTableEntry(prefix, key);
        }

        internal static bool HasIDTableEntry(string prefix, string key)
        {
            IDTableEntry idTableEntry = IDTable.GetID(prefix, key);
            return idTableEntry != null;
        }

        #endregion
    }
}