using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Glass.Mapper.Sc;
using Projects.Common.Glass;
using Projects.Common.Utils;
using Projects.Models.Glass.Reboot.Containers;
using Projects.Models.Glass.Reboot.Items;
using Projects.Reboot.DataMigration.Utils;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.People;
using Movie = TMDbLib.Objects.Movies.Movie;
using Person = TMDbLib.Objects.People.Person;
using Profile = TMDbLib.Objects.General.Profile;
using PersonModel = Projects.Models.Glass.Reboot.Items.Person;

namespace Projects.Reboot.DataMigration.Managers
{
    class PeopleManager:ManagerBase, IPeopleManager
    {

        private readonly ISitecoreService _masterService;
        private readonly People _peopleRootFolder;
        private readonly CastAndCrew _castAndCreweRootFolder;

        public PeopleManager(ISitecoreService masterService)
        {
            _masterService = masterService;
            _peopleRootFolder = masterService.GetItem<People>(RebootConstants.PeopleRootID.Guid);
            _castAndCreweRootFolder = masterService.GetItem<CastAndCrew>(RebootConstants.CastCrewRootID.Guid);
        }

        public PersonModel AddPerson(Person p)
        {
            PersonModel person = Mapper.Map<PersonModel>(p);
            AddImage(p, person);
            return person.Save(_peopleRootFolder, _masterService);
        }

        public void AddImage(Person p, PersonModel person)
        {

            if (p.Images != null && p.Images.Profiles != null && p.Images.Profiles.Any())
            {
                List<Profile> profiles = p.Images.Profiles;
                string filePath = profiles.First().FilePath;
                Guid dbImageId = MediaExtensions.AddMovieDBImage(_client, filePath);
                person.Image = GlassExtensions.GetImage(dbImageId, person.Title);
            }
        }

        public IEnumerable<PersonModel> AddPeople(IEnumerable<Person> people)
        {
            return people.Select(AddPerson).ToList();
        }

        public ICrewMember AddCrewMember(Crew c)
        {
            int id = c.Id;
            Person p = _client.GetPerson(id, PersonMethods.Images);
            PersonModel person = AddPerson(p);
            ICrewMember crew = Mapper.Map<CrewMember>(c);
            crew.Person = GlassExtensions.GetInternalLink(person);
            crew.Save(_castAndCreweRootFolder, _masterService);
            return crew;
        }

        public ICastMember AddCastMember(Cast c)
        {
            int id = c.Id;
            Person p = _client.GetPerson(id, PersonMethods.Images);
            PersonModel person = AddPerson(p);
            ICastMember castMember = Mapper.Map<CastMember>(c);
            castMember.Person= GlassExtensions.GetInternalLink(person);
            castMember.Save(_castAndCreweRootFolder, _masterService);
            return castMember;
        }

        public IEnumerable<ICrewMember> AddCrew(IEnumerable<Crew> crew)
        {
            return crew.Select(AddCrewMember).ToList();
        }

        public IEnumerable<ICastMember> AddCast(IEnumerable<Cast> cast)
        {
            return cast.Select(AddCastMember).ToList();

            
        }

        public void AddCrewToMovie(Movie movie, IMovie importedMovie)
        {
            List<Crew> crew = movie.Credits.Crew;
            if (crew.Any())
            {
                IEnumerable<ICrewMember> crewMembers = AddCrew(crew);
                importedMovie.Crews = crewMembers.AsItemIDs();
            }
        }

        public void AddCastToMovie(Movie movie, IMovie importedMovie)
        {
            List<Cast> cast = movie.Credits.Cast;
            if (cast.Any())
            {
                IEnumerable<ICastMember> castMembers = AddCast(cast);
                importedMovie.Casts = castMembers.AsItemIDs();
            }
        }
    }

    interface IPeopleManager : IDataMigrationManager
    {
        PersonModel AddPerson(Person person);
        IEnumerable<PersonModel> AddPeople(IEnumerable<Person> people);

        ICrewMember AddCrewMember(Crew c);
        ICastMember AddCastMember(Cast crew);

        IEnumerable<ICrewMember> AddCrew(IEnumerable<Crew> crew);

       IEnumerable<ICastMember> AddCast(IEnumerable<Cast> cast);


        void AddCrewToMovie(Movie movie, IMovie importedMovie);
        void AddCastToMovie(Movie movie, IMovie importedMovie);
    }
}
