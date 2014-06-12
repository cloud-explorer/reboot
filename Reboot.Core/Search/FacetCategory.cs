#region

using System;
using System.Collections.Generic;
using System.Linq;
using Projects.Reboot.Common;
using Projects.Reboot.Contracts;

#endregion

namespace Projects.Reboot.Core.Search
{
    public class FacetCategory 
    {
        #region C'tors

        public FacetCategory(string name, IEnumerable<Sitecore.ContentSearch.Linq.FacetValue> values)
        {
            var commonTextService = new ServiceFactory().GetService<ICommonTextService>();
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            if (values == null)
            {
                throw new ArgumentNullException("values");
            }
            FieldName = name;
            Name = commonTextService.GetTextFor(name);
            Values = values.Select(v => (FacetValue) v).ToList();
        }

        #endregion

        #region Instance Properties

        public string Name { get; protected set; }

        public string FieldName { get; protected set; }

        public List<FacetValue> Values { get; protected set; }

        #endregion
    }
}