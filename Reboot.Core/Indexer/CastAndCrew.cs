using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glass.Mapper.Sc;
using Projects.Models.Glass.Reboot.Items;
using Projects.Reboot.Common;
using Projects.Common.Glass;
using Sitecore.Configuration;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;

namespace Projects.Reboot.Core.Indexer
{
    public class CastAndCrew : IComputedIndexField
    {
        public ISitecoreService SitecoreContext { get; set; }
        public CastAndCrew(ISitecoreService context)
        {
            SitecoreContext = context;
        }

        public CastAndCrew()
        {
            SitecoreContext = new SitecoreService(Sitecore.Context.Database);
        }

        public object ComputeFieldValue(IIndexable indexable)
        {
            SitecoreIndexableItem item = indexable as SitecoreIndexableItem;
            if (item == null || !item.Item.TemplateID.Equals(IMovieConstants.TemplateId)) return null;
            Movie m = SitecoreContext.GetItem<Movie>(item.Item.ID.Guid);

            List<string> castCrew = (m.Crews.Select(c => SitecoreContext.GetItem<CrewMember>(c))
                .Where(crew => crew != null)
                .Select(crew => crew.GetLinkedItem<Person, CrewMember>(crew.Person))
                .Where(p => p != null)
                .Select(p => p.Title)).ToList();
            castCrew.AddRange((m.Casts.Select(c => SitecoreContext.GetItem<CastMember>(c))
                .Where(c => c != null)
                .Select(c => c.GetLinkedItem<Person, CastMember>(c.Person))
                .Where(p => p != null)
                .Select(p => p.Title)).ToList());

            return castCrew;
        }

        public string FieldName { get; set; }

        public string ReturnType { get; set; }
    }
}
