#region

using System;
using System.Collections.Generic;
using System.Linq;
using Glass.Mapper.Sc;
using Projects.Models;
using Projects.Models.Glass.Reboot.Items;
using Projects.Models.Glass.Reboot.RenderingParameters;
using Projects.Reboot.Contracts;
using Projects.Reboot.Core.Search;
using Sitecore.ContentSearch.Linq;

#endregion

namespace Projects.Reboot.Services
{
    public class MovieSearchService : SearchServiceBase, IMovieSearchService
    {
        private readonly ISiteSearchService _siteSearchService;

        #region C'tors

        public MovieSearchService(ISitecoreContext context, ISiteSearchService siteSearchService)
            : base(context)
        {
            _siteSearchService = siteSearchService;
        }

        public MovieSearchService(ISiteSearchService siteSearchService)
            : base()
        {
            _siteSearchService = siteSearchService;
        }

        public MovieSearchService()
        {
            _siteSearchService = new SiteSearchService();
        }
        #endregion

        #region IMovieSearchService Members

        public IQueryable<Movie> GetComingSoonMovies(int take = 5)
        {
            return GetMoviesByReleaseDate(DateTime.Now, DateTime.Now.AddDays(30))
                .OrderBy(m => m.ReleaseDate)
                .Take(take);
        }

        public IQueryable<Movie> GetNowRunningMovies(int take = 5)
        {
            return GetMoviesByReleaseDate(DateTime.Now.AddDays(-30), DateTime.Now)
                .OrderByDescending(m => m.ReleaseDate)
                .Take(take);
        }

        /// <summary>
        ///     Get the movies released between the specifed set of dates
        /// </summary>
        /// <param name="releaseDateStart"></param>
        /// <param name="releaseDateEnd"></param>
        /// <returns></returns>
        public IQueryable<Movie> GetMoviesByReleaseDate(DateTime releaseDateStart, DateTime releaseDateEnd)
        {
            IQueryable<Movie> queryable;
            using (var context = Index.CreateSearchContext())
            {
                queryable = context.GetQueryable<Movie>().Where(m => m.ReleaseDate.Between(releaseDateStart, releaseDateEnd, Inclusion.Both));
            }
            return queryable;
        }

        public IEnumerable<Movie> GetMoviesByPopularity(SearchQuery query, SearchParameter parameter, out int totalResultCount)
        {
            var results = _siteSearchService.GetSearchResultsAs<Movie>(
                //Where Conditions
                w =>
                {
                    w = w.Where(SearchHelper.GetPredicate<Movie>(parameter, _context));
                    w = w.Where(m => m.ReleaseDate < DateTime.Now);
                    if (!string.IsNullOrEmpty(query.Keyword))
                    {
                        //w = w.Where(m => m.Crews)
                    }
                    return w;
                }
                //Facet Set up
                , f => f
                //Sort order setup
                , s =>
                {
                    s = s.OrderByDescending(m => m.Popularity)
                        .ThenByDescending(m => m.VoteAverage)
                        .ThenByDescending(m => m.ReleaseDate);
                    return s;
                },
                query
                );
            totalResultCount = results.TotalSearchResults;
            IEnumerable<Movie> movies = results.Hits.Select(h => _context.GetItem<Movie>(h.Document.Id));
            return movies;
        }

        #endregion
    }
}