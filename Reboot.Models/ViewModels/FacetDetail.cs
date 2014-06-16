using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Projects.Reboot.Core.Search;

namespace Projects.Models.ViewModels
{
    public class FacetDetail
    {
        public FacetCategory Category { get; set; }

        public FacetValue Value { get; set; }
    }
}
