#region

using Projects.Common.Contracts;
using Projects.Models.Glass.Reboot.Containers;
using Projects.Reboot.Common;
using Projects.Reboot.DataMigration.Services;
using Sitecore;
using Sitecore.Shell.Framework.Commands;

#endregion

namespace Projects.Reboot.DataMigration.Commands
{
    public class ImportMovies : Command
    {
        #region Readonly & Static Fields

        private readonly IServiceFactory _serviceFactory;

        #endregion

        #region C'tors

        public ImportMovies()
        {
            _serviceFactory = new ServiceFactory();
        }

        #endregion

        #region Instance Methods

        public override void Execute([NotNull] CommandContext context)
        {
            IDataMigrationService ms = _serviceFactory.GetService<IDataMigrationService>();
            ms.StartDataMigration();
        }

        public override CommandState QueryState(CommandContext context)
        {
            return context.Items[0].TemplateID.Equals(IMoviesConstants.TemplateId)
                       ? CommandState.Enabled
                       : CommandState.Hidden;
        }

        #endregion
    }
}