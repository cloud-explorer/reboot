using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Glass.Mapper.Sc;
using Projects.Models.Glass.Reboot.Items;
using Projects.Models.Glass.Reboot.RenderingParameters;
using Sitecore.Configuration;
using Sitecore.ContentSearch.Linq.Utilities;
using Sitecore.Data;

namespace Projects.Reboot.Core.Search
{
    public static class SearchHelper
    {
        public static Expression<Func<T, bool>> GetPredicate<T>(ISearchParameter parameters, ISitecoreContext context) where T : IFacetableContent
        {
            var predicate = PredicateBuilder.True<T>();
            predicate = parameters.FilterOnTemplates.Aggregate(predicate, (current, temp) => current.Or(p => p.TemplateId == temp));
            if (parameters.FilterOnFields == null || !parameters.FilterOnFields.Any()) return predicate;
            IEnumerable<Guid> facetOns = parameters.FilterOnFields;
            IEnumerable<Models.Glass.Reboot.Facet> facets =
                facetOns.Select(i => context.GetItem<Models.Glass.Reboot.Facet>(i)).Reverse();
            foreach (var filter in facets)
            {
                string s = HttpContext.Current.Request.QueryString.Get(filter.Name);
                switch (filter.FacetName)
                {
                    case "Genres":
                        if (!string.IsNullOrEmpty(s) && ShortID.IsShortID(s))
                        {
                            //w = w.Where(a => a.Genres.Contains(Guid.Parse(s)));
                            predicate = predicate.And(o => o.Genres.Contains(Guid.Parse(s)));
                        }
                        break;
                    case "Production Company":
                        if (!string.IsNullOrEmpty(s) && ShortID.IsShortID(s))
                        {
                            //w = w.Where(a => a.ProductionCompanies.Contains(Guid.Parse(s)));
                            predicate = predicate.And(a => a.ProductionCompanies.Contains(Guid.Parse(s)));
                        }
                        break;
                    case "Status":
                        if (!string.IsNullOrEmpty(s))
                        {
                            //w = w.Where(a => a.Status.Equals(s));
                            predicate = predicate.And(a => a.Status.Equals(s));
                        }
                        break;
                    case "Spoken Language":
                        if (!string.IsNullOrEmpty(s))
                        {
                            //w = w.Where(a => a.Status.Equals(s));
                            predicate = predicate.And(a => a.SpokenLanguages.Contains(Guid.Parse(s)));
                        }
                        break;
                }

            }
            return predicate;
        }
    }
}
