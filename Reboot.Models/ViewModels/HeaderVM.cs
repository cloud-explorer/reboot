using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glass.Mapper.Sc.Fields;
using Projects.Models.Glass.Common;

namespace Projects.Models.ViewModels
{
    public class HeaderVM
    {
        public Image BrandImage { get; set; }

        public Link LinkToHome { get; set; }

        public IEnumerable<PageBase> Navigationitems { get; set; } 
    }
}
