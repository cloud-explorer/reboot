using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Glass.Mapper.Sc;
using Glass.Mapper.Sc.Web.Mvc;
using Projects.Models.Glass.Common;
using Projects.Reboot.Common;
using Projects.Reboot.Contracts;

namespace Projects.Website.Controllers
{
    public abstract class BaseController : GlassController
    {
        protected ICommonTextService TextService;

        protected BaseController(ICommonTextService textService)
        {
            TextService = textService;
        }

        protected BaseController()
        {
            TextService = new ServiceFactory().GetService<ICommonTextService>();
        }

        ///// <summary>
        ///// Redirect to Knowledge Gateway Home Page
        ///// </summary>
        ///// <returns></returns>
        public ActionResult RedirectToHomePage()
        {
            PageBase homeItem = SitecoreContext.GetHomeItem<PageBase>();
            string homePageUrl = homeItem.Url;
            return Redirect(homePageUrl);
        }        
	}
}