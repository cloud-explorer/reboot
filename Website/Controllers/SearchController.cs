using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Castle.Core.Internal;
using Castle.MicroKernel.ModelBuilder.Descriptors;
using Glass.Mapper.Sc.Configuration.Attributes;
using Projects.Models;
using Projects.Models.Glass;
using Projects.Models.Glass.Common;
using Projects.Models.Glass.Reboot.Items;
using Projects.Models.Glass.Reboot.RenderingParameters;
using Projects.Models.ViewModels;
using Projects.Reboot.Common;
using Projects.Reboot.Contracts;
using Sitecore.ContentSearch.Linq;
using Sitecore.ContentSearch.Utilities;
using Sitecore.ContentSearch.Linq.Utilities;
using Sitecore.Data;
using Sitecore.Diagnostics;
using Sitecore.Mvc.Extensions;
using Sitecore.Shell.Applications.Templates.TemplateBuilder;

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

        public ActionResult FacetList(SearchQuery query)
        {
            var parameters = GetRenderingParameters<FacetParameters>();
            FacetResults results = GetFacetsForParameters(parameters, query);
            Reboot.Core.Search.FacetResults facets = results;
            return View(facets);
        }

        private FacetResults GetFacetsForParameters(IFacetParameters parameters, SearchQuery query)
        {
            
           
            IEnumerable<Guid> facetOns = parameters.FilterOnFields;
            IEnumerable<Models.Glass.Reboot.Facet> facets =
                facetOns.Select(i => SitecoreContext.GetItem<Models.Glass.Reboot.Facet>(i)).Reverse();
           // predicate = predicate.And(a => a.Status.Equals("released"));
            
            FacetResults results = _siteSearchService.GetFacetResultsAs<FacetableContent>(
                //Where Conditions
                w =>
                {
                    var predicate = Reboot.Core.Search.SearchHelper.GetPredicate<FacetableContent>(parameters, SitecoreContext);
                    w = w.Where(predicate);
                    return w;
                }
                //Facet Set up
                , f =>
                {
                    foreach (var facet in facets)
                    {
                        switch (facet.FacetName)
                        {
                            case "Genres":
                                f = f.FacetOn(o => o.Genres, 1);
                                break;
                            case "Production Company":
                                f = f.FacetOn(o => o.ProductionCompanies, 1);
                                break;
                            case "Status":
                                f = f.FacetOn(o => o.Status, 1);
                                break;
                            case "Spoken Language":
                                f = f.FacetOn(o => o.SpokenLanguages, 1);
                                break;
                        }
                    }

                    //Expression<Func<FacetableContent, object>> expression = o => (o.GetType().InvokeMember(parameters.FacetBy, BindingFlags.GetProperty, null, o, null));
                    //f =f.FacetOn(expression, 1);
                    return f;
                }
                //Sort order setup
                , s => s
                    ,query
                );
            return results;
        }
    }
}