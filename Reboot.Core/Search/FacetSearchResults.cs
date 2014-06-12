#region

using System;
using System.Collections.Generic;
using Sitecore.ContentSearch.Linq;

#endregion

namespace Projects.Reboot.Core.Search
{
    public class FacetSearchResults<TSource>
    {
        #region C'tors

        public FacetSearchResults(IEnumerable<SearchHit<TSource>> results, int totalSearchResults)
        {
            if (results == null)
            {
                throw new ArgumentNullException("results");
            }
            Hits = results;
            TotalSearchResults = totalSearchResults;
        }

        public FacetSearchResults(IEnumerable<SearchHit<TSource>> results, int totalSearchResults,
                                  FacetResults facets = null)
            : this(results, totalSearchResults)
        {
            Facets = facets;
        }

        #endregion

        #region Instance Properties

        public FacetResults Facets { get; private set; }

        public IEnumerable<SearchHit<TSource>> Hits { get; private set; }

        public int TotalSearchResults { get; private set; }

        #endregion

        #region Operators

        public static implicit operator FacetSearchResults<TSource>(SearchResults<TSource> actualSearchResult)
        {
            return actualSearchResult != null
                       ? new FacetSearchResults<TSource>(actualSearchResult.Hits, actualSearchResult.TotalSearchResults,
                                                         actualSearchResult.Facets)
                       : null;
        }

        #endregion
    }
}