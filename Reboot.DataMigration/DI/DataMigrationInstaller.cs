#region

using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Glass.Mapper.Sc;
using Projects.Reboot.DataMigration.Managers;
using Projects.Reboot.DataMigration.Services;

#endregion

namespace Projects.Reboot.DataMigration.DI
{
    public class DataMigrationInstaller : IWindsorInstaller
    {
        #region IWindsorInstaller Members

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<IDataMigrationService>()
                         .ImplementedBy<DataMigrationService>()
                         .LifestyleTransient(), 
                         Component.For<IGenreManager>()
                         .ImplementedBy<GenreManager>()
                         .DependsOn(Dependency.OnComponent(typeof (ISitecoreService), "glass.service.master"))
                         .LifestyleTransient(),
                Component.For<ILanguageManager>()
                         .ImplementedBy<LanguageManager>()
                         .DependsOn(Dependency.OnComponent(typeof (ISitecoreService), "glass.service.master"))
                         .LifestyleTransient(),
                Component.For<IPeopleManager>()
                         .ImplementedBy<PeopleManager>()
                         .DependsOn(Dependency.OnComponent(typeof (ISitecoreService), "glass.service.master"))
                         .LifestyleTransient(),
                Component.For<IMovieManager>()
                         .ImplementedBy<MovieManager>()
                         .DependsOn(Dependency.OnComponent(typeof (ISitecoreService), "glass.service.master"))
                         .LifestyleTransient()
                );
        }

        #endregion
    }
}