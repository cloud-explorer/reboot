using System;
using System.Linq;
using Projects.Common.Contracts;
using Projects.Models;
using Projects.Models.Glass.Common;
using Projects.Models.Glass.Reboot;
using Projects.Models.Glass.Reboot.Items;
using Sitecore.ContentSearch.Linq;

namespace Projects.Reboot.Contracts
{
    public interface ISiteSearchService : IServiceContract
    {
        SearchResults<T> GetSearchResultsAs<T>(Func<IQueryable<T>, IQueryable<T>> whereSnippet
            , Func<IQueryable<T>, IQueryable<T>> facetSnippet
            , Func<IQueryable<T>, IQueryable<T>> sortSnippet
            , SearchQuery query = null) where T : class, ISearchableContent;

        FacetResults GetFacetResultsAs<T>(Func<IQueryable<T>, IQueryable<T>> whereSnippet
            , Func<IQueryable<T>, IQueryable<T>> facetSnippet
            , Func<IQueryable<T>, IQueryable<T>> sortSnippet
            , SearchQuery query = null) where T : class, ISearchableContent;
    }
}