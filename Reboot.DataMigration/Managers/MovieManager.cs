#region

using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Glass.Mapper.Sc;
using Projects.Common.Contracts;
using Projects.Common.Glass;
using Projects.Common.Utils;
using Projects.Models.Glass.Reboot.Containers;
using Projects.Models.Glass.Reboot.Items;
using Projects.Reboot.DataMigration.Utils;
using TMDbLib.Objects.Discover;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.Search;
using Movie = TMDbLib.Objects.Movies.Movie;
using ProductionCompany = TMDbLib.Objects.Movies.ProductionCompany;
using Trailers = TMDbLib.Objects.Movies.Trailers;
using GenreModel = Projects.Models.Glass.Reboot.Items.Genre;
using MovieModel = Projects.Models.Glass.Reboot.Items.Movie;
using ProductionCompanyModel = Projects.Models.Glass.Reboot.Items.ProductionCompany;
using TrailerModel = Projects.Models.Glass.Reboot.Items.Trailer;
using TrailersModel = Projects.Models.Glass.Reboot.Containers.Trailers;
#endregion

namespace Projects.Reboot.DataMigration.Managers
{
    internal interface IMovieManager : IDataMigrationManager
    {
        #region Instance Methods

        void Add(int id);
        void Add(Movie movie);
        void AddBackdrop(Movie m, MovieModel movie);
        void AddPoster(Movie m, MovieModel movie);
        void AddProductionCompany(Movie m, MovieModel movie);
        void AddTrailers(Movie m, MovieModel movie);
        SearchContainer<SearchMovie> Discover(DateTime startDate);
        SearchContainer<SearchMovie> Discover(DateTime startDate, int page = 0);

        SearchContainer<SearchMovie> Discover(DateTime startDate, DateTime endDate, string language = "en",
                                              DiscoverMovieSortBy movieSortBy =
                                                  DiscoverMovieSortBy.ReleaseDateDescending,
                                              int voteAverageGreaterThan = 5,
                                              int page = 0);

        Movie Get(int id);
        List<Cast> GetCast(Movie movie);
        List<Cast> GetCast(int id);
        List<Crew> GetCrew(Movie movie);
        List<Crew> GetCrew(int id);

        List<GenreModel> GetGenres(Movie m);
        void Save(MovieModel movie);

        #endregion

        void AddGenres(Movie movie, MovieModel importedMovie);
        MovieModel GetMappedMovie(Movie movie);
    }

    internal sealed class MovieManager : ManagerBase, IMovieManager
    {
        #region Readonly & Static Fields

        private readonly ISitecoreService _masterService;
        private readonly ISitecoreContext _context;
        private readonly Movies _moviesRootFolder;
        private readonly TrailersModel _trailersRootFolder;
        private readonly ProductionCompanies _prodCompaniesRootFolder;

        #endregion

        #region C'tors

        public MovieManager(ISitecoreService masterService, ISitecoreContext context)
        {
            _masterService = masterService;
            _context = context;
            _moviesRootFolder = masterService.GetItem<Movies>(RebootConstants.MovieRootID.Guid);
            _trailersRootFolder = masterService.GetItem<TrailersModel>(RebootConstants.TrailerRootID.Guid);
            _prodCompaniesRootFolder = masterService.GetItem<ProductionCompanies>(RebootConstants.ProductionCompaniesRootID.Guid);
        }

        #endregion

        #region Instance Methods

        public void AddProductionCompany(List<ProductionCompany> c, MovieModel movie)
        {
            List<Guid> companies = new List<Guid>();
            foreach (IProductionCompany comp in c.Select(Mapper.Map<ProductionCompanyModel>))
            {
                comp.Save(_prodCompaniesRootFolder, _masterService);
                if (comp.Id != Guid.Empty) companies.Add(comp.Id);
            }
            if (companies.Any()) movie.ProductionCompanies = companies;
        }

        public void AddTrailers(Trailers t, MovieModel movie)
        {
            List<Guid> trailers = new List<Guid>();
            foreach (TrailerModel trailer in t.Youtube.Select(Mapper.Map<TrailerModel>))
            {
                trailer.ExternalId = t.Id.ToString();
                trailer.Save(_trailersRootFolder, _masterService);
                if (trailer.Id != Guid.Empty) trailers.Add(trailer.Id);
            }
            if (trailers.Any()) movie.Trailers = trailers;
        }

        public Movie GetMovie(SearchMovie m)
        {
            return Get(m.Id);
        }

       

        #endregion

        #region MovieModelManager Members

        public void Add(int id)
        {
            Movie movie = Get(id);
            Add(movie);
        }

        public void Add(Movie m)
        {
            if (m == null) return;
            MovieModel movie = Mapper.Map<MovieModel>(m);
            movie.Save(_moviesRootFolder, _masterService);
        }

        public void Save(MovieModel movie)
        {
            movie.Save(_moviesRootFolder, _masterService);
        }

        public void AddGenres(Movie movie, MovieModel importedMovie)
        {
            List<GenreModel> genres = GetGenres(movie);
            if (genres.Any())
            {
                importedMovie.Genres = genres.AsItemIDs();
            }
        }

        public MovieModel GetMappedMovie(Movie movie)
        {
            return Mapper.Map<MovieModel>(movie);

        }

        public void AddTrailers(Movie m, MovieModel movie)
        {
            AddTrailers(m.Trailers, movie);
        }

        public void AddProductionCompany(Movie m, MovieModel movie)
        {
            AddProductionCompany(m.ProductionCompanies, movie);
        }

        public Movie Get(int id)
        {
            return _client.GetMovie(id,
                                    MovieMethods.Credits | MovieMethods.Images | MovieMethods.Keywords |
                                    MovieMethods.Trailers);
        }

        public List<Crew> GetCrew(Movie movie)
        {
            return GetCrew(movie.Id);
        }

        public List<Crew> GetCrew(int id)
        {
            Credits movieCredits = _client.GetMovieCredits(id);
            if (movieCredits != null) return movieCredits.Crew;
            return new List<Crew>();
        }

        public List<Cast> GetCast(Movie movie)
        {
            return GetCast(movie.Id);
        }

        public List<Cast> GetCast(int id)
        {
            Credits movieCredits = _client.GetMovieCredits(id);
            if (movieCredits != null) return movieCredits.Cast;
            return new List<Cast>();
        }

        public List<GenreModel> GetGenres(Movie m)
        {
            List<GenreModel> genres =
                m.Genres.Select(genre => new GenreModel(_context).GetItemFromKey(genre.Id.ToString())).ToList();

            return genres;
        }

        public void AddPoster(Movie m, MovieModel movie)
        {
            if (m.Images != null && m.Images.Posters != null && m.Images.Posters.Any())
            {
                IOrderedEnumerable<ImageData> imageDatas = m.Images.Posters.OrderByDescending(i => i.VoteAverage);
                Guid posterId = AddImage(imageDatas.First());
                movie.Image = GlassExtensions.GetImage(posterId, movie.Title);
            }
        }

        public void AddBackdrop(Movie m, MovieModel movie)
        {
            if (m.Images != null && m.Images.Backdrops != null && m.Images.Backdrops.Any())
            {
                IOrderedEnumerable<ImageData> imageDatas = m.Images.Backdrops.OrderByDescending(i => i.VoteAverage);
                Guid posterId = AddImage(imageDatas.First());
                movie.Backdrop = GlassExtensions.GetImage(posterId, movie.Title);
            }
        }


        public SearchContainer<SearchMovie> Discover(DateTime startDate)
        {
            return Discover(startDate, DateTime.Now);
        }

        public SearchContainer<SearchMovie> Discover(DateTime startDate, int page = 0)
        {
            return Discover(startDate, DateTime.Now, page: page);
        }

        public SearchContainer<SearchMovie> Discover(DateTime startDate, DateTime endDate, string language = "en",
                                                     DiscoverMovieSortBy movieSortBy =
                                                         DiscoverMovieSortBy.ReleaseDateDescending,
                                                     int voteAverageGreaterThan = 5,
                                                     int page = 0)
        {
            SearchContainer<SearchMovie> movies = _client.DiscoverMovies(language: language
                                                                         , voteAverageGreaterThan: voteAverageGreaterThan
                                                                         , releaseDateGreaterThan: startDate
                                                                         , releaseDateLessThan: endDate
                                                                         , sortBy:DiscoverMovieSortBy.ReleaseDateDescending
                                                                         , page: page);
            return movies;
        }

        #endregion
    }
}