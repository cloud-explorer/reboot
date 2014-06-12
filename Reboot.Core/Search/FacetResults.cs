#region

using System.Collections.Generic;
using System.Linq;

#endregion

namespace Projects.Reboot.Core.Search
{
    public class FacetResults
    {
        #region Readonly & Static Fields

        private List<FacetCategory> categories = new List<FacetCategory>();

        #endregion

        #region Instance Properties

        public List<FacetCategory> Categories
        {
            get { return categories; }
            set { categories = value; }
        }

        #endregion

        public static implicit operator FacetResults(Sitecore.ContentSearch.Linq.FacetResults facets)
        {
            return facets != null ? new FacetResults
                {
                    Categories = facets.Categories.Select( s=> new FacetCategory(s.Name, s.Values)).ToList()
                } : null;
        }


    }
}