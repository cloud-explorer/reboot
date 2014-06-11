#region

using Glass.Mapper.Sc;
using Projects.Common.Contracts;
using Sitecore.ContentSearch;

#endregion

namespace Projects.Reboot.Services
{
    public class SearchServiceBase
    {
        #region Readonly & Static Fields

        protected readonly ISitecoreContext _context;

        #endregion

        #region C'tors

        public SearchServiceBase(ISitecoreContext context)
        {
            _context = context;
        }

        public SearchServiceBase()
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
                if (currentDatabase.ToLower() == "core") currentDatabase = "master";
                string indexName = string.Format("sitecore_{0}_index", currentDatabase);
                ISearchIndex index = ContentSearchManager.GetIndex(indexName);
                return index;
            }
        }

        #endregion
    }
}