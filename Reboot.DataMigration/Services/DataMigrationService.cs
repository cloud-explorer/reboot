#region

using System;
using AutoMapper;
using Projects.Common.Contracts;
using Projects.Models.Glass;
using Projects.Models.Glass.Common.FieldSections;
using Projects.Models.Glass.Reboot.Items;
using Projects.Reboot.DataMigration.Managers;
using Projects.Reboot.DataMigration.MappingProfiles;
using Projects.Reboot.DataMigration.Utils;
using Sitecore.Data;
using Sitecore.Diagnostics;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;
using Movie = TMDbLib.Objects.Movies.Movie;
using MovieModel = Projects.Models.Glass.Reboot.Items.Movie;

#endregion

namespace Projects.Reboot.DataMigration.Services
{
    public interface IDataMigrationService : IServiceContract
    {
        void StartDataMigration();
    }

    internal sealed class DataMigrationService : IDataMigrationService
    {
        #region Readonly & Static Fields

        private readonly IGenreManager _genreManager;
        private readonly ILanguageManager _languageManager;
        private readonly IMovieManager _movieManager;
        private readonly IPeopleManager _peopleManager;

        #endregion

        #region Fields

        private int _maxItemCount;
        private bool isFullRefresh = false;

        #endregion

        #region C'tors

        public DataMigrationService(IMovieManager movieManager
                                      , IGenreManager gm
                                      , ILanguageManager languageManager
                                      , IPeopleManager peopleManager)
        {
            if (movieManager == null)
                throw new ApplicationException("Movie Manager not initialized");
            _movieManager = movieManager;
            _genreManager = gm;
            _languageManager = languageManager;
            _peopleManager = peopleManager;
            Mapper.Initialize(x => x.AddProfile<DomainMapper>());
        }

        #endregion

        #region Instance Methods

        public void StartDataMigration()
        {
            #region Load Refernce Data

            //Bring the reference Data
            //This is the only reference data I have
            //You could preload all reference data here

            ImportGeneres();

            #endregion

            using (new BulkUpdateContext())
            {
                ImportMovies();
            }
        }

        private void AddCast(Movie movie, MovieModel importedMovie)
        {
            ExecuteWithExceptionHandling(() => _peopleManager.AddCastToMovie(movie, importedMovie)
                , string.Format("An error occurred while creating cast for movie {0} with id {1}", movie.Title, movie.Id.ToString()));

        }

        private void AddCrew(Movie movie, MovieModel importedMovie)
        {
            ExecuteWithExceptionHandling(() => _peopleManager.AddCrewToMovie(movie, importedMovie)
                , string.Format("An error occurred while creating crew for movie {0} with id {1}", movie.Title, movie.Id.ToString()));
        }

        private void AddGenres(Movie movie, MovieModel importedMovie)
        {
            ExecuteWithExceptionHandling(() => _movieManager.AddGenres(movie, importedMovie)
                 , string.Format("An error occurred while creating Genres for movie {0} with id {1}", movie.Title, movie.Id.ToString()));
        }

        private void AddImages(Movie movie, MovieModel importedMovie)
        {
            ExecuteWithExceptionHandling(() =>
                {
                    _movieManager.AddPoster(movie, importedMovie);
                    _movieManager.AddBackdrop(movie, importedMovie);
                }
                , string.Format("An error occurred while creating images for movie {0} with id {1}", movie.Title, movie.Id.ToString()));
        }

        private void AddLanguageToMovie(Movie movie, MovieModel importedMovie)
        {
            ExecuteWithExceptionHandling(() => _languageManager.AddLanguagesToMovie(movie, importedMovie)
                , string.Format("An error occurred while creating languages for movie {0} with id {1}", movie.Title, movie.Id.ToString()));
        }

        private void AddProductionCompanies(Movie movie, MovieModel importedMovie)
        {
            ExecuteWithExceptionHandling(() => _movieManager.AddProductionCompany(movie, importedMovie)
                , string.Format("An error occurred while creating production companies for movie {0} with id {1}", movie.Title, movie.Id.ToString()));
        }

        private void AddTrailers(Movie movie, MovieModel importedMovie)
        {
            ExecuteWithExceptionHandling(() => _movieManager.AddTrailers(movie, importedMovie)
                , string.Format("An error occurred while creating trailers for movie {0} with id {1}", movie.Title, movie.Id.ToString()));
        }

        private void ImportGeneres()
        {
            ExecuteWithExceptionHandling(() => _genreManager.AddAll());
        }


        private void ImportMovies()
        {
            _maxItemCount = 1000;
            DateTime now = DateTime.Now;
            DateTime startYear = new DateTime(now.Year - 2, now.Month, now.Day);
            SearchContainer<SearchMovie> movies = _movieManager.Discover(startYear);
            int resultCount = movies.TotalResults;
            if (_maxItemCount > resultCount)
            {
                _maxItemCount = resultCount;
            }
            int totalPageCount = _maxItemCount/RebootConstants.PageSize;

            for (int i = 0; i < totalPageCount;)
            {
                foreach (var m in movies.Results)
                {
                    string title = "Unknown";
                    string id = Guid.Empty.ToString();
                    try
                    {
                        Movie movie = _movieManager.Get(m.Id);
                        title = movie.Title;
                        id = movie.Id.ToString();
                        if (string.IsNullOrEmpty(id)
                            || string.IsNullOrEmpty(title)
                            || IDTableExtesions.HasIDTableEntry<MovieModel>(id)) continue;
                        MovieModel importedMovie = _movieManager.GetMappedMovie(movie);
                        AddGenres(movie, importedMovie);
                        AddLanguageToMovie(movie, importedMovie);
                        AddCrew(movie, importedMovie);
                        AddCast(movie, importedMovie);
                        AddImages(movie, importedMovie);
                        AddTrailers(movie, importedMovie);
                        AddProductionCompanies(movie, importedMovie);
                        //Finally Add the movie to the database
                        _movieManager.Save(importedMovie);

                    }
                    catch (Exception ex)
                    {
                        Log.Error(string.Format("An error occurred while creating movie {0} with id {1}", title, id), ex,
                                  title);
                    }
                }
                i++;
                movies = _movieManager.Discover(startYear, i);
            }
        }


        /// <summary>
        /// This is not the best way to handle exceptions. This is just a quick and dirty way to get the data into Sitecore
        /// </summary>
        /// <param name="codetoExecute"></param>
        /// <param name="errorToLog"></param>

        private void ExecuteWithExceptionHandling(Action codetoExecute, string errorToLog = "An unhandled exception occurred")
        {
            try
            {
                codetoExecute.Invoke();
            }
            catch (Sitecore.Exceptions.InvalidItemNameException nameException)
            {
                Log.Error("Name error occurred while creating/renaming item", nameException, codetoExecute);
                throw;
            }
            catch (Sitecore.Exceptions.InvalidValueException valueException)
            {
                Log.Error("Invalid value error occurred while creating/renaming item", valueException, codetoExecute);
                throw;
            }
            catch (Exception ex)
            {
                Log.Error(errorToLog, ex, codetoExecute);
            }
        }

        #endregion
    }
}