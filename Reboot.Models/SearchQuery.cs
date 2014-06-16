using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Projects.Models.Glass.Reboot;
using Projects.Models.Glass.Reboot.RenderingParameters;
using Sitecore.Shell.Framework.Commands;

namespace Projects.Models
{
    public class SearchQuery
    {
        public string Keyword { get; set; }

        //public IEnumerable<Facet> Facets { get; set; }

        /// <summary>
        /// The search will be restricted to only items that are based on these templates
        /// </summary>
        //public IEnumerable<Guid> TemplateIds { get; set; }
        public FacetParameters FacetParameters { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }
    }
}
