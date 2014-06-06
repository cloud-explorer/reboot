#region

using System.Web.Optimization;
using Sitecore;
using Sitecore.Pipelines;

#endregion

namespace Projects.Reboot.Core.MVC
{
    public class RegisterBundles
    {
        [UsedImplicitly]
        public virtual void Process(PipelineArgs args)
        {
#if DEBUG
            BundleTable.EnableOptimizations = false;
#else
            BundleTable.EnableOptimizations = true; 
#endif
            Register(BundleTable.Bundles);
        }

        private void Register(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/sc_ignore_bundles/rebootscripts").Include(
               "~/js/jquery-{version}.js",
               "~/js/bootstrap.js",
               "~/js/modern-business.js",
                "~/js/reboot.js",
                "~/js/jquery.rateit.min.js"
               ));

            bundles.Add(new StyleBundle("~/sc_ignore_bundles/rebootstyles").Include(
                "~/css/bootstrap.css",
                "~/css/theme.css",
                "~/font-awesome/css/font-awesome.min.css",
                "~/css/modern-business.css",
                "~/css/reboot.css",
                "~/css/rateit.css"
                ));

           
        }
    }
}
