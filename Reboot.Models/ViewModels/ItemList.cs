using System.Collections;
using System.Collections.Generic;
using Projects.Models.Glass;
using Projects.Models.Glass.Common;

namespace Projects.Models.ViewModels
{
    public class ItemList
    {
        public string ListName { get; set; } 
        public string IconClassName { get; set; } 
        public IEnumerable<IPageBase> Items {get; set; }
    }
}