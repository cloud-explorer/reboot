#region

using Glass.Mapper.Sc;
using Projects.Common.Contracts;
using Sitecore.ContentSearch;

#endregion

namespace Projects.Reboot.Services
{
    public class SearchService
    {
        #region Readonly & Static Fields

        protected readonly ISitecoreContext _context;

        #endregion

        #region C'tors

        public SearchService(ISitecoreContext context)
        {
            _context = context;
        }

        public SearchService()
        {
            _context = new SitecoreContext();
        }

        #endregion

        #region Instance Properties

        public ISearchIndex Index
        {
            get
            {
                string currentDatabase = _context.Database.Name;
                string indexName = string.Format("sitecore_{0}_index", currentDatabase);
                ISearchIndex index = ContentSearchManager.GetIndex(indexName);
                return index;
            }
        }

        #endregion
    }
}