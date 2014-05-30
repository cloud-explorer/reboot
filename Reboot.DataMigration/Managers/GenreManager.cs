#region

using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Glass.Mapper.Sc;
using Projects.Common.Contracts;
using Projects.Models.Glass.Reboot.Containers;
using Projects.Models.Glass.Reboot.Items;
using Projects.Reboot.DataMigration.Utils;
using Sitecore.Data.IDTables;
using Genre = TMDbLib.Objects.General.Genre;
using GenreModel = Projects.Models.Glass.Reboot.Items.Genre;

#endregion

namespace Projects.Reboot.DataMigration.Managers
{
    internal class GenreManager : ManagerBase, IGenreManager
    {
        #region Readonly & Static Fields

        private readonly Genres _genreRootFolder;
        private readonly ISitecoreService _masterService;

        #endregion

        #region C'tors

        public GenreManager(ISitecoreService masterService)
        {
            _masterService = masterService;
            _genreRootFolder = masterService.GetItem<Genres>(RebootConstants.GenresRootID.Guid);
        }

        #endregion

        #region Instance Methods

        //public string GetFlattenedGenreIDs(IEnumerable<Genre> genres, string seperator = "|")
        //{
        //    if (!genres.Any()) return string.Empty;
        //    return GetFlattenedGenreIDs(genres.Select(i => i.Id), seperator);
        //}

        //public string GetFlattenedGenreIDs(IEnumerable<int> ids, string seperator = "|")
        //{
        //    if (!ids.Any()) return string.Empty;
        //    foreach (int id in ids)
        //    {
        //    }
        //    return string.Join(seperator, ids);
        //}

        #endregion

        #region IGenreManager Members

        public void AddAll()
        {
            List<Genre> genres = _client.GetGenres();
            AddRange(genres);
        }

        public void Add(Genre genre)
        {
            GenreModel g = Mapper.Map<GenreModel>(genre);
            g.Save(_genreRootFolder, _masterService);
        }

        public void AddRange(IEnumerable<Genre> genres)
        {
            if (genres == null || !genres.Any()) return;
            foreach (Genre genre in genres.ToList())
            {
                Add(genre);
            }
        }
        
        public IEnumerable<Guid> GetGenreIDs(IEnumerable<int> ids)
        {
            List<Guid> genreIDs =
                (ids.Select(id => IDTable.GetID(typeof(GenreModel).ToString(), id.ToString()))
                    .Where(idTableEntry => idTableEntry != null)
                    .Select(idTableEntry => idTableEntry.ID.Guid)).ToList();
            return genreIDs;
        }

        public IEnumerable<Guid> GetGenreIDs(IEnumerable<Genre> genres)
        {
            return GetGenreIDs(genres.Select(g => g.Id));
        }

        #endregion
    }

    internal interface IGenreManager : IDataMigrationManager
    {
        #region Instance Methods

        void AddAll();
        void Add(Genre genre);
        void AddRange(IEnumerable<Genre> genres);
        IEnumerable<Guid> GetGenreIDs(IEnumerable<int> ids);
        IEnumerable<Guid> GetGenreIDs(IEnumerable<Genre> genres);

        #endregion
    }
}