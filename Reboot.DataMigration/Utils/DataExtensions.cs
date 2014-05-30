#region

using System;
using System.Collections.Generic;
using System.Linq;
using Glass.Mapper.Sc;
using Projects.Models.Glass;
using Projects.Models.Glass.Common.FieldSections;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.SecurityModel;

#endregion

namespace Projects.Reboot.DataMigration.Utils
{
    internal static class DataExtensions
    {
        #region Readonly & Static Fields

        private static bool _isFullRefresh = false;

        #endregion

        #region Class Methods

        public static IEnumerable<Guid> AsItemIDs<T>(this IEnumerable<T> objs) where T : class, IGlassBase
        {
            List<Guid> itemIds = new List<Guid>();
            var col = objs.ToList();
            if (col.Any())
            {
                itemIds.AddRange(objs.Where(obj => obj != null
                                                    && !obj.Id.Equals(Guid.Empty))
                                    .Select(obj => obj.Id)
                                    .ToList());
            }

            return itemIds;
        }

        public static bool IsUpdateRequired<T>(this T obj) where T : class, IHasExternalId
        {
            //This is just hard-coded now
            //There can be logic here to figure out if update is required
            return _isFullRefresh;
        }


        public static T Save<T>(this T obj, IGlassBase parent, ISitecoreService service) where T : class, IHasExternalId
        {

            if (obj.HasIDTableEntry())
            {
                if (obj.IsUpdateRequired())
                {
                    //      Any data manipulation can go hear before
                    //      updating the database once again
                    //      This case will never be hit in this sample 
                    using (new SecurityDisabler())
                    {
                        service.Save(obj);
                    }
                }
                obj = service.GetItem<T>(obj.GetItemIdFromIDTableEntry().Guid);
            }
            else
            {
                obj.Name = ItemUtil.ProposeValidItemName(obj.Name).ToLower().Replace(" ", "-");
                using (new SecurityDisabler())
                {
                    service.Create(parent, obj);
                }
            }

            return obj;
        }

        #endregion
    }
}