#region

using System;
using Projects.Models.Glass.Common.FieldSections;
using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Items;

#endregion

namespace Projects.Reboot.Core.Search
{
    public class FacetValue
    {
        #region C'tors

        public FacetValue(string name, int aggregate)
        {
            Name = name;
            Aggregate = aggregate;
            if (Item != null) ItemName = Item[IHasTitleConstants.TitleFieldName];
        }

        #endregion

        #region Instance Properties

        public int Aggregate { get; protected set; }

        public string ItemName { get; protected set; }

        public string Name { get; protected set; }

        public Item Item
        {
            get { return Context.Database.GetItem(ItemId); }
        }

        private ID ItemId
        {
            get
            {
                Guid id;
                if (!Guid.TryParse(Name, out id))
                {
                    return ID.Null;
                }
                return ID.Parse(id);
            }
        }

        #endregion

        public static implicit operator FacetValue(Sitecore.ContentSearch.Linq.FacetValue facet)
        {
            return facet != null ? new FacetValue(facet.Name, facet.Aggregate) : null;
        }

        public static implicit operator Sitecore.ContentSearch.Linq.FacetValue(FacetValue facet)
        {
            return facet != null ? new Sitecore.ContentSearch.Linq.FacetValue(facet.Name, facet.Aggregate) : null;
        }
    }
}