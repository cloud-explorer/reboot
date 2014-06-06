using System;
using System.Collections.Generic;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Glass.Mapper.Configuration;
using Glass.Mapper.Sc.CastleWindsor;
using Projects.Reboot.Common;
using Projects.Reboot.Core.DI;
using Projects.Reboot.DataMigration.DI;

namespace Projects.Website.App_Start
{
    public static  class GlassMapperScCustom
    {
		public static void CastleConfig(IWindsorContainer container){

            var config = new Config {UseWindsorContructor = true};
		    List<IWindsorInstaller> installers = new List<IWindsorInstaller>
                {
                    new SitecoreInstaller(config), new RebootInstaller(), new ControllerInstaller()
                };
            #if DEBUG
            installers.Add(new DataMigrationInstaller());
            #endif

		    IWindsorContainer windsorContainer = container.Install(installers.ToArray());
            ObjectBase.Container = windsorContainer;
		    //container.Register(Classes.FromAssemblyInDirectory(new AssemblyFilter("bin"))
		    //                            .Where(t => t.Name.StartsWith("Projects.") 
		    //                                        && t.IsAssignableFrom(typeof(IWindsorInstaller)))
		    //                            .WithService.DefaultInterfaces()
		    //                            .LifestyleSingleton());
		    //List<IWindsorInstaller> installers = new List<IWindsorInstaller>{new SitecoreInstaller(config)};
		    //installers.AddRange(container.ResolveAll<IWindsorInstaller>());
		    //container.Install(installers.ToArray());
		}

		public static IConfigurationLoader[] GlassLoaders(){			
			
			/* USE THIS AREA TO ADD FLUENT CONFIGURATION LOADERS
             * 
             * If you are using Attribute Configuration or automapping/on-demand mapping you don't need to do anything!
             * 
             */

			return new IConfigurationLoader[]{};
		}
		public static void PostLoad(){
			//Remove the comments to activate CodeFist
			/* CODE FIRST START
            var dbs = Sitecore.Configuration.Factory.GetDatabases();
            foreach (var db in dbs)
            {
                var provider = db.GetDataProviders().FirstOrDefault(x => x is GlassDataProvider) as GlassDataProvider;
                if (provider != null)
                {
                    using (new SecurityDisabler())
                    {
                        provider.Initialise(db);
                    }
                }
            }
             * CODE FIRST END
             */

            
		}
    }
}
