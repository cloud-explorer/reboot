#region

using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Glass.Mapper.Sc;
using Projects.Common.Contracts;
using Projects.Models.Glass.Reboot.Containers;
using Projects.Models.Glass.Reboot.Items;
using Projects.Reboot.DataMigration.Utils;
using TMDbLib.Objects.Movies;
using Movie = TMDbLib.Objects.Movies.Movie;
using LanguageModel = Projects.Models.Glass.Reboot.Items.Language;
#endregion

namespace Projects.Reboot.DataMigration.Managers
{
    internal interface ILanguageManager : IDataMigrationManager
    {
        #region Instance Methods

        Language Add(SpokenLanguage language);
        IEnumerable<Language> AddRange(IEnumerable<SpokenLanguage> languages);

        #endregion

        void AddLanguagesToMovie(Movie movie, IMovie importedMovie);
    }

    internal class LanguageManager : ILanguageManager
    {
        #region Readonly & Static Fields

        private readonly Languages _langRootFolder;
        private readonly ISitecoreService _masterService;

        #endregion

        #region C'tors

        public LanguageManager(ISitecoreService masterService)
        {
            _masterService = masterService;
            _langRootFolder = masterService.GetItem<Languages>(RebootConstants.LanguageRootID.Guid);
            ;
        }

        #endregion

        #region ILanguageManager Members

        public Language Add(SpokenLanguage language)
        {
            if (language == null) return null;
            Language movie = Mapper.Map<Language>(language);
            return movie.Save(_langRootFolder, _masterService);
        }

        public IEnumerable<Language> AddRange(IEnumerable<SpokenLanguage> languages)
        {
            List<Language> langs = languages.Select(Add).ToList();
            return langs;
        }

        public void AddLanguagesToMovie(Movie movie, IMovie importedMovie)
        {
            //Add Language
            if (movie.SpokenLanguages.Any())
            {
                IEnumerable<Language> languages = AddRange(movie.SpokenLanguages);
                importedMovie.SpokenLanguages = languages.AsItemIDs();
            }
        }

        #endregion
    }
}