using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Glass.Mapper.Sc;
using Glass.Mapper.Sc.Fields;
using Projects.Models.Glass.Common;
using Projects.Models.ViewModels;

namespace Projects.Website.Controllers
{
    public class PageController : BaseController
    {
        public PageController(ISitecoreContext context) : base(context)
        {
        }

        public ActionResult Header()
        {
            SiteHome homeItem = Context.GetHomeItem<SiteHome>();
            HeaderVM hvm = new HeaderVM
                {
                    LinkToHome = new Link
                        {
                            TargetId = homeItem.Id,
                            Text = homeItem.Title,
                            Type = LinkType.Internal
                        },
                };
            return View();
        }
	}
}