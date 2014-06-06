using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Sitecore.Mvc.Controllers;

namespace Projects.Reboot.Core.DI
{
    public class ControllerInstaller : IWindsorInstaller
    {
        /// <summary>
        /// Installs the controllers
        /// </summary>
        /// <param name="container"></param>
        /// <param name="store"></param>
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(FindControllers().Configure(x => x.LifestyleTransient()));
        }

        /// <summary>
        /// Find controllers within this assembly in the same namespace as HomeController
        /// </summary>
        /// <returns></returns>
        private BasedOnDescriptor FindControllers()
        {
            return Classes.FromAssemblyInThisApplication()
                    .BasedOn<SitecoreController>();
        }
    }
}
