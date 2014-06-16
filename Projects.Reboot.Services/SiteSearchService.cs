#region

using System;
using System.Linq;
using Projects.Common.Core;
using Projects.Models;
using Projects.Models.Glass.Common;
using Projects.Models.Glass.Reboot;
using Projects.Models.Glass.Reboot.Items;
using Projects.Reboot.Contracts;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Linq;

#endregion

namespace Projects.Reboot.Services
{
    public class SiteSearchService : SearchServiceBase, ISiteSearchService
    {
        #region ISiteSearchService Members

        public SearchResults<T> GetSearchResultsAs<T>(Func<IQueryable<T>, IQueryable<T>> whereSnippet
            , Func<IQueryable<T>, IQueryable<T>> facetSnippet
            , Func<IQueryable<T>, IQueryable<T>> sortSnippet
            , SearchQuery query = null) where T : class, ISearchableContent
        {
            SearchResults<T> results;
            if(query == null) query = new SearchQuery();
            using (var context = Index.CreateSearchContext())
            {
                var queryable = SetupSearchCriteria(whereSnippet, facetSnippet, sortSnippet, query, context);
                if (query.PageNumber > 0)
                {
                    queryable = queryable.Skip((query.PageNumber - 1)*query.PageSize);
                }
                if (query.PageSize > 0)
                {
                    queryable = queryable.Take(query.PageSize);
                }
               
                results = queryable.GetResults(GetResultsOptions.GetScores);
            }
            return results;
        }

        public FacetResults GetFacetResultsAs<T>(Func<IQueryable<T>, IQueryable<T>> whereSnippet
            , Func<IQueryable<T>, IQueryable<T>> facetSnippet
            , Func<IQueryable<T>, IQueryable<T>> sortSnippet
            , SearchQuery query = null) where T : class, ISearchableContent
        {
            FacetResults results;
            if (query == null) query = new SearchQuery();
            using (var context = Index.CreateSearchContext())
            {
                var queryable = SetupSearchCriteria(whereSnippet, facetSnippet, sortSnippet, query, context);
                results = queryable.GetFacets();
            }
            return results;
        }

        private static IQueryable<T> SetupSearchCriteria<T>(Func<IQueryable<T>, IQueryable<T>> whereSnippet, Func<IQueryable<T>, IQueryable<T>> facetSnippet, Func<IQueryable<T>, IQueryable<T>> sortSnippet,
            SearchQuery query, IProviderSearchContext context) where T : class, IPageBase, ISearchableContent
        {
            IQueryable<T> queryable = context.GetQueryable<T>();
            //Make sure only items marked a Searchable are searched
            queryable = queryable.Where(s => s.BaseTemplates.Contains(ISearchableContentConstants.TemplateId.Guid));
            //Make sure you return results only for the type requested
            Guid templateId = typeof (T).GetTemplateIdFromType();
            if (!templateId.Equals(Guid.Empty))
            {
                //Apply the template filter so that only items of the specified type will be returned
                queryable = queryable.Where(s => s.BaseTemplates.Contains(templateId));
            }
            if (!string.IsNullOrEmpty(query.Keyword))
            {
                queryable = queryable.Where(s => s.Title.Like(query.Keyword, 0f).Boost(5)
                                                 || s.Description.Like(query.Keyword, 0f));
            }
            //Apply the where conditions that we passed in
            queryable = whereSnippet.Invoke(queryable);
            //apply the facets that were requested
            queryable = facetSnippet.Invoke(queryable);
            //apply any sorting rules specified
            queryable = sortSnippet.Invoke(queryable);
            return queryable;
        }

        #endregion
    }
}