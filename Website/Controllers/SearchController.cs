using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Castle.Core.Internal;
using Castle.MicroKernel.ModelBuilder.Descriptors;
using Glass.Mapper.Sc.Configuration.Attributes;
using Projects.Common.Utils;
using Projects.Models;
using Projects.Models.Glass;
using Projects.Models.Glass.Common;
using Projects.Models.Glass.Reboot.Items;
using Projects.Models.Glass.Reboot.RenderingParameters;
using Projects.Reboot.Common;
using Projects.Reboot.Contracts;
using Sitecore.ContentSearch.Linq;
using Sitecore.ContentSearch.Utilities;
using Sitecore.ContentSearch.Linq.Utilities;
using Sitecore.Diagnostics;
using Sitecore.Mvc.Extensions;

namespace Projects.Website.Controllers
{
    public class SearchController : BaseController
    {
        private readonly ISiteSearchService _siteSearchService;

        public SearchController(ISiteSearchService siteSearchService)
        {
            _siteSearchService = siteSearchService;
        }

        public SearchController()
        {
            _siteSearchService = new ServiceFactory().GetService<ISiteSearchService>();
        }

        public ActionResult Facet()
        {
            var parameters = GetRenderingParameters<FacetingParameters>();
            
            FacetResults results = GetFacetsForParameters(parameters);
            Reboot.Core.Search.FacetResults facets = results;
            return View(facets);
        }

        private FacetResults GetFacetsForParameters(FacetingParameters parameters)
        {
            var predicate = PredicateBuilder.True<FacetableContent>();
            predicate = parameters.TemplatesToFacets.Aggregate(predicate, (current, temp) => current.Or(w => w.TemplateId == temp));

            FacetResults results = _siteSearchService.GetFacetResultsAs<FacetableContent>(
                //Where Conditions
                w =>
                {
                    w = w.Where(predicate);
                    return w;
                }
                //Facet Set up
                , f =>
                {
                    switch (parameters.FacetBy)
                    {
                        case "Genres":
                            f = f.FacetOn(o => o.Genres, 1);
                            break;
                        case "Status":
                            f = f.FacetOn(o => o.Status, 1);
                            break;
                    }
                    //Expression<Func<FacetableContent, object>> expression = o => (o.GetType().InvokeMember(parameters.FacetBy, BindingFlags.GetProperty, null, o, null));
                    //f =f.FacetOn(expression, 1);
                    return f;
                }
                //Sort order setup
                , s => s
                
                );
            return results; 
        }
    }
}