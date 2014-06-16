#region

using System.Globalization;
using System.Linq;
using AutoMapper;
using Projects.Common.Core;
using Projects.Models.Glass.Reboot.Items;
using Projects.Reboot.DataMigration.Utils;
using Sitecore.Data.Items;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;
using Genre = TMDbLib.Objects.General.Genre;
using Movie = TMDbLib.Objects.Movies.Movie;
using Person = TMDbLib.Objects.People.Person;
using ProductionCompany = TMDbLib.Objects.Movies.ProductionCompany;
using Profile = AutoMapper.Profile;

using GenreModel = Projects.Models.Glass.Reboot.Items.Genre;
using LanguageModel = Projects.Models.Glass.Reboot.Items.Language;
using TrailerModel = Projects.Models.Glass.Reboot.Items.Trailer;
using ProductionCompanyModel = Projects.Models.Glass.Reboot.Items.ProductionCompany;
using PersonModel = Projects.Models.Glass.Reboot.Items.Person;
using MovieModel = Projects.Models.Glass.Reboot.Items.Movie;

#endregion

namespace Projects.Reboot.DataMigration.MappingProfiles
{
    public class DomainMapper : Profile
    {
        #region Instance Properties

        public override string ProfileName
        {
            get { return "DomainMapper"; }
        }

        #endregion

        #region Instance Methods

        protected override void Configure()
        {
            //Genre Mappings
            Mapper.CreateMap<Genre, GenreModel>()
                  .ForMember(m => m.Name, opt => opt.MapFrom(m => ItemUtil.ProposeValidItemName(m.Name.Trim())))
                  .ForMember(m => m.Title, opt => opt.MapFrom(m => m.Name))
                  .ForMember(m => m.ExternalId, opt => opt.MapFrom(m => m.Id.ToString(CultureInfo.InvariantCulture)))
                  .ForMember(m => m.Version, opt => opt.Ignore())
                  .ForMember(m => m.Id, opt => opt.Ignore())
                  .ForMember(m => m.Language, opt => opt.Ignore())
                  .ForMember(m => m.TemplateId, opt => opt.Ignore())
                  .ForMember(m => m.TemplateName, opt => opt.Ignore())
                  .ForMember(m => m.BaseTemplates, opt => opt.Ignore())
                  .ForMember(m => m.Children, opt => opt.Ignore())
                  .ForMember(m => m.Parent, opt => opt.Ignore())
                  .ForMember(m => m.Context, opt => opt.Ignore())
                  .ForMember(m => m.Url, opt => opt.Ignore())
                  .ForAllMembers(p => p.Condition(c => !c.IsSourceValueNull));

            //Language Mappings
            Mapper.CreateMap<SpokenLanguage, LanguageModel>()
                  .ForMember(m => m.Name, opt => opt.MapFrom(m => ItemUtil.ProposeValidItemName(m.Iso_639_1.Trim())))
                  .ForMember(m => m.Title, opt => opt.MapFrom(m => m.Name))
                  .ForMember(m => m.ExternalId, opt => opt.MapFrom(m => m.Iso_639_1))
                  .ForMember(m => m.Version, opt => opt.Ignore())
                  .ForMember(m => m.Id, opt => opt.Ignore())
                  .ForMember(m => m.Language, opt => opt.Ignore())
                  .ForMember(m => m.TemplateId, opt => opt.Ignore())
                  .ForMember(m => m.TemplateName, opt => opt.Ignore())
                  .ForMember(m => m.BaseTemplates, opt => opt.Ignore())
                  .ForMember(m => m.Children, opt => opt.Ignore())
                  .ForMember(m => m.Parent, opt => opt.Ignore())
                  .ForMember(m => m.Context, opt => opt.Ignore())
                  .ForMember(m => m.Url, opt => opt.Ignore())
                  .ForAllMembers(p => p.Condition(c => !c.IsSourceValueNull));

            //Crew Mappings
            Mapper.CreateMap<Crew, CrewMember>()
                  .ForMember(m => m.Name, opt => opt.MapFrom(m => ItemUtil.ProposeValidItemName(m.Name.Trim())))
                  .ForMember(m => m.ExternalId, opt => opt.MapFrom(m => m.Id))
                  .ForMember(m => m.Person, opt => opt.Ignore())
                  .ForMember(m => m.Version, opt => opt.Ignore())
                  .ForMember(m => m.Id, opt => opt.Ignore())
                  .ForMember(m => m.Language, opt => opt.Ignore())
                  .ForMember(m => m.TemplateId, opt => opt.Ignore())
                  .ForMember(m => m.TemplateName, opt => opt.Ignore())
                  .ForMember(m => m.BaseTemplates, opt => opt.Ignore())
                  .ForMember(m => m.Children, opt => opt.Ignore())
                  .ForMember(m => m.Parent, opt => opt.Ignore())
                  .ForMember(m => m.Context, opt => opt.Ignore())
                  .ForMember(m => m.Url, opt => opt.Ignore())
                  .ForAllMembers(p => p.Condition(c => !c.IsSourceValueNull));

            //Cast Mappings
            Mapper.CreateMap<Cast, CastMember>()
                  .ForMember(m => m.Name, opt => opt.MapFrom(m => ItemUtil.ProposeValidItemName(m.Name.Trim())))
                  .ForMember(m => m.ExternalId, opt => opt.MapFrom(m => m.Id))
                  .ForMember(m => m.SortOrder, opt => opt.MapFrom(m => m.Order))
                  .ForMember(m => m.Person, opt => opt.Ignore())
                  .ForMember(m => m.Version, opt => opt.Ignore())
                  .ForMember(m => m.Id, opt => opt.Ignore())
                  .ForMember(m => m.Language, opt => opt.Ignore())
                  .ForMember(m => m.TemplateId, opt => opt.Ignore())
                  .ForMember(m => m.TemplateName, opt => opt.Ignore())
                  .ForMember(m => m.BaseTemplates, opt => opt.Ignore())
                  .ForMember(m => m.Children, opt => opt.Ignore())
                  .ForMember(m => m.Parent, opt => opt.Ignore())
                  .ForMember(m => m.Context, opt => opt.Ignore())
                  .ForMember(m => m.Url, opt => opt.Ignore())
                  .ForAllMembers(p => p.Condition(c => !c.IsSourceValueNull));

            //Trailer Mappings
            Mapper.CreateMap<Youtube, TrailerModel>()
                  .IgnoreAllNonExisting()
                  .ForMember(m => m.Name, opt => opt.MapFrom(m => ItemUtil.ProposeValidItemName(m.Name.Trim())))
                  .ForMember(m => m.Title, opt => opt.MapFrom(m => m.Name))
                  .ForMember(m => m.ExternalId, opt => opt.MapFrom(m => m.Source))
                  .ForMember(m => m.Version, opt => opt.Ignore())
                  .ForMember(m => m.Id, opt => opt.Ignore())
                  .ForMember(m => m.Language, opt => opt.Ignore())
                  .ForMember(m => m.TemplateId, opt => opt.Ignore())
                  .ForMember(m => m.TemplateName, opt => opt.Ignore())
                  .ForMember(m => m.BaseTemplates, opt => opt.Ignore())
                  .ForMember(m => m.Children, opt => opt.Ignore())
                  .ForMember(m => m.Parent, opt => opt.Ignore())
                  .ForMember(m => m.Context, opt => opt.Ignore())
                  .ForMember(m => m.Url, opt => opt.Ignore())
                  .ForAllMembers(p => p.Condition(c => !c.IsSourceValueNull));

            //Production Mappings
            Mapper.CreateMap<ProductionCompany, ProductionCompanyModel>().IgnoreAllNonExisting()
                  .ForMember(m => m.Name, opt => opt.MapFrom(m => ItemUtil.ProposeValidItemName(m.Name.Trim())))
                  .ForMember(m => m.Title, opt => opt.MapFrom(m => m.Name))
                  .ForMember(m => m.ExternalId, opt => opt.MapFrom(m => m.Id))
                  .ForMember(m => m.Version, opt => opt.Ignore())
                  .ForMember(m => m.Id, opt => opt.Ignore())
                  .ForMember(m => m.Language, opt => opt.Ignore())
                  .ForMember(m => m.TemplateId, opt => opt.Ignore())
                  .ForMember(m => m.TemplateName, opt => opt.Ignore())
                  .ForMember(m => m.BaseTemplates, opt => opt.Ignore())
                  .ForMember(m => m.Children, opt => opt.Ignore())
                  .ForMember(m => m.Parent, opt => opt.Ignore())
                  .ForMember(m => m.Context, opt => opt.Ignore())
                  .ForMember(m => m.Url, opt => opt.Ignore())
                  .ForAllMembers(p => p.Condition(c => !c.IsSourceValueNull));

            //Person Mappings
            Mapper.CreateMap<Person, PersonModel>()
                  .ForMember(m => m.Name, opt => opt.MapFrom(m => ItemUtil.ProposeValidItemName(m.Name)))
                  .ForMember(m => m.Title, opt => opt.MapFrom(m => m.Name))
                  .ForMember(m => m.AlsoKnownAs, opt => opt.MapFrom(m => m.AlsoKnownAs.Count > 1 ? string.Join(", ", m.AlsoKnownAs) : m.AlsoKnownAs.FirstOrDefault()))
                  .ForMember(m => m.Description, opt => opt.MapFrom(m => m.Biography))
                  .ForMember(m => m.MenuTitle, opt => opt.MapFrom(m => m.Name))
                  .ForMember(m => m.IsAdult, opt => opt.MapFrom(m => m.Adult))
                  .ForMember(m => m.BreadcrumbTitle, opt => opt.MapFrom(m => m.Name))
                  .ForMember(m => m.ExternalId, opt => opt.MapFrom(m => m.Id))
                  .ForMember(m => m.Keywords, opt => opt.MapFrom(m => string.Join(", ", m.AlsoKnownAs) + ", " + m.Name))
                  .ForMember(m => m.MetaDescription, opt => opt.MapFrom(m => m.Biography.Truncate(150, true, false)))
                  .ForMember(m => m.Image, opt => opt.Ignore())
                  .ForMember(m => m.Version, opt => opt.Ignore())
                  .ForMember(m => m.Id, opt => opt.Ignore())
                  .ForMember(m => m.Language, opt => opt.Ignore())
                  .ForMember(m => m.TemplateId, opt => opt.Ignore())
                  .ForMember(m => m.TemplateName, opt => opt.Ignore())
                  .ForMember(m => m.BaseTemplates, opt => opt.Ignore())
                  .ForMember(m => m.Children, opt => opt.Ignore())
                  .ForMember(m => m.Parent, opt => opt.Ignore())
                  .ForMember(m => m.Context, opt => opt.Ignore())
                  .ForMember(m => m.Url, opt => opt.Ignore())
                  .ForMember(m => m.CssClassName, opt => opt.Ignore())
                  .ForMember(m => m.MenuHoverText, opt => opt.Ignore())
                  .ForMember(m => m.ShowInMenu, opt => opt.Ignore())
                  .ForAllMembers(p => p.Condition(c => !c.IsSourceValueNull));

            //Movie Mappings
            Mapper.CreateMap<Movie, MovieModel>()
                  .ForMember(m => m.Name, opt => opt.MapFrom(m => ItemUtil.ProposeValidItemName(m.Title.Trim())))
                  .ForMember(m => m.MenuTitle, opt => opt.MapFrom(m => m.Title))
                  .ForMember(m => m.ExternalId, opt => opt.MapFrom(m => m.Id))
                  .ForMember(m => m.BreadcrumbTitle, opt => opt.MapFrom(m => m.Title))
                  .ForMember(m => m.Keywords,
                             opt => opt.MapFrom(m => string.Join(";", m.Keywords.Keywords.Select(k => k.Name))))
                  .ForMember(m => m.MetaDescription,
                             opt => opt.MapFrom(m => m.Overview.Truncate(150, true, false)))
                  .ForMember(m => m.Description, opt => opt.MapFrom(m => m.Overview))
                  .ForMember(m => m.Genres, opt => opt.Ignore())
                  .ForMember(m => m.SpokenLanguages, opt => opt.Ignore())
                  .ForMember(m => m.Casts, opt => opt.Ignore())
                  .ForMember(m => m.Crews, opt => opt.Ignore())
                  .ForMember(m => m.Backdrop, opt => opt.Ignore())
                  .ForMember(m => m.ProductionCompanies, opt => opt.Ignore())
                  .ForMember(m => m.Image, opt => opt.Ignore())
                  .ForMember(m => m.Version, opt => opt.Ignore())
                  .ForMember(m => m.Trailers, opt => opt.Ignore())
                  .ForMember(m => m.Id, opt => opt.Ignore())
                  .ForMember(m => m.Language, opt => opt.Ignore())
                  .ForMember(m => m.TemplateId, opt => opt.Ignore())
                  .ForMember(m => m.TemplateName, opt => opt.Ignore())
                   .ForMember(m => m.BaseTemplates, opt => opt.Ignore())
                  .ForMember(m => m.Children, opt => opt.Ignore())
                  .ForMember(m => m.Parent, opt => opt.Ignore())
                  .ForMember(m => m.Context, opt => opt.Ignore())
                  .ForMember(m => m.Url, opt => opt.Ignore())
                  .ForMember(m => m.CssClassName, opt => opt.Ignore())
                  .ForMember(m => m.MenuHoverText, opt => opt.Ignore())
                  .ForMember(m => m.ShowInMenu, opt => opt.Ignore())
                  .ForAllMembers(p => p.Condition(c => !c.IsSourceValueNull));
        }

        #endregion
    }
}