using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Glass.Mapper.Sc;
using Projects.Common.Contracts;
using Projects.Reboot.Common;

namespace Projects.Reboot.Core.DI
{
    public class RebootInstaller : IWindsorInstaller
    {
         
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<ISitecoreService>()
                                       .ImplementedBy<SitecoreService>()
                                       .DependsOn(Dependency.OnValue("databaseName", "core"))
                                       .Named("glass.service.core")
                                       .LifestyleTransient(),
                              Component.For<ISitecoreService>()
                                       .ImplementedBy<SitecoreService>()
                                       .DependsOn(Dependency.OnValue("databaseName", "master"))
                                       .Named("glass.service.master")
                                       .LifestyleTransient(),
                              Component.For<ISitecoreService>()
                                       .ImplementedBy<SitecoreService>()
                                       .DependsOn(Dependency.OnValue("databaseName", "web"))
                                       .Named("glass.service.web")
                                       .LifestyleTransient(),
                              Component.For<ISitecoreContext>()
                                       .ImplementedBy<SitecoreContext>()
                                       .LifestyleTransient(),
                               Component.For<IServiceFactory>()
                                       .ImplementedBy<ServiceFactory>()
                                       .LifestylePerWebRequest());
        }
    }
}