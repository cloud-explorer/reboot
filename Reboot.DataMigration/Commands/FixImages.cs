using System;
using System.Linq;
using System.Reflection;
using Glass.Mapper.Sc;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using Projects.Common.Utils;
using Projects.Models;
using Projects.Models.Glass.Common;
using Projects.Models.Glass.Common.FieldSections;
using Projects.Models.Glass.Reboot;
using Projects.Models.Glass.Reboot.Containers;
using Projects.Models.Glass.Reboot.Items;
using Projects.Reboot.Common;
using Projects.Reboot.Contracts;
using Projects.Reboot.Services;
using Sitecore.ContentSearch.Linq;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Shell.Framework.Commands;

namespace Projects.Reboot.DataMigration.Commands
{
    public class FixImages : Command
    {
        private ISiteSearchService _siteSearchService;
        private readonly ISitecoreService _sitecoreContext;

        public FixImages(ISiteSearchService siteSearchService, ISitecoreService sitecoreContext)
        {
            _siteSearchService = siteSearchService;
            _sitecoreContext = sitecoreContext;
        }

        public FixImages()
        {
            _siteSearchService = new SiteSearchService();
            _sitecoreContext = new SitecoreService("master");
        }
        public override void Execute(CommandContext context)
        {
            ResetFieldValue<Movie>();
            ResetFieldValue<Person>();
        }

        private void ResetFieldValue<T>() where T : class, IPageBase, ISearchableContent
        {
            SearchResults<T> results = _siteSearchService.GetSearchResultsAs<T>(w => w, f => f, s => s);
            foreach (SearchHit<T> hit in results.Hits)
            {
                Item m = _sitecoreContext.GetItem<Item>(hit.Document.Id);
                if (m == null)
                {
                    Log.Warn(string.Format("Could not find item with id {0}", hit.Document.Id), results);
                    continue;
                }
                ImageField imf = m.Fields[IHasImageConstants.ImageFieldId];
                if (imf.MediaItem != null) continue;
                m.Editing.BeginEdit();
                try
                {
                    imf.InnerField.Reset();
                }
                catch (Exception ex)
                {
                    Log.Error(string.Format("Error occured while processing item with id {0}", hit.Document.Id), ex,
                        results);
                    m.Editing.CancelEdit();
                }
                finally
                {
                    m.Editing.EndEdit();
                }


            }
        }

        public override CommandState QueryState(CommandContext context)
        {
            ID templateID = context.Items[0].TemplateID;
            return (templateID.Equals(IMoviesConstants.TemplateId)
                || templateID.Equals(IPeopleConstants.TemplateId))
                       ? CommandState.Enabled
                       : CommandState.Hidden;
        }
    }
}