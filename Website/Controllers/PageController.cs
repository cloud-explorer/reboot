using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Glass.Mapper.Sc;
using Glass.Mapper.Sc.Fields;
using Projects.Models.Glass;
using Projects.Models.Glass.Common;
using Projects.Models.ViewModels;

namespace Projects.Website.Controllers
{
    public class PageController : BaseController
    {
        public ActionResult Header()
        {
            SiteHome homeItem = SitecoreContext.GetHomeItem<SiteHome>();
            return View(homeItem);
        }
    }
}