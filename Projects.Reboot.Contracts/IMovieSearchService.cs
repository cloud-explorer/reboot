#region

using System;
using System.Collections.Generic;
using System.Linq;
using Projects.Common.Contracts;
using Projects.Models;
using Projects.Models.Glass.Reboot.Items;
using Projects.Models.Glass.Reboot.RenderingParameters;

#endregion

namespace Projects.Reboot.Contracts
{
    public interface IMovieSearchService : IServiceContract
    {
        #region Instance Methods

        IQueryable<Movie> GetComingSoonMovies(int take = 5);

        /// <summary>
        ///     Get the movies released between the specifed set of dates
        /// </summary>
        /// <param name="releaseDateStart"></param>
        /// <param name="releaseDateEnd"></param>
        /// <returns></returns>
        IQueryable<Movie> GetMoviesByReleaseDate(DateTime releaseDateStart, DateTime releaseDateEnd);

        IQueryable<Movie> GetNowRunningMovies(int take = 5);

        IEnumerable<Movie> GetMoviesByPopularity(SearchQuery query, SearchParameter parameter, out int totalResultCount);

        #endregion
    }
}