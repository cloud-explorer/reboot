using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Glass.Mapper.Sc;
using Glass.Mapper.Sc.Fields;
using Projects.Models.Glass;
using Projects.Models.Glass.Common;
using Projects.Models.Glass.Common.Components;
using Projects.Models.ViewModels;
using Sitecore.Buckets.Search;
using Projects.Common.Glass;

namespace Projects.Website.Controllers
{
    public class PageController : BaseController
    {
        public ViewResult Header()
        {
            SiteHome homeItem = SitecoreContext.GetHomeItem<SiteHome>();
            return View(homeItem);
        }

        public ViewResult Carousel()
        {
            Carousel carousel = GetControllerItem<Carousel>();
            List<Slide> slides = carousel.GetChildren<Slide>().ToList();
            return View(slides);
        }

        public ActionResult RenderItemList(ItemList list)
        {
            if (list == null || ! list.Items.Any()) return RedirectToNotFound();
            return View(list);
        }

        public ViewResult Breadcrumb()
        {
            PageBase datasourceItem = GetControllerItem<PageBase>();
            IEnumerable<PageBase> navigableAncestors = datasourceItem.GetNavigableAncestors();
            return View(navigableAncestors);
        }
    }
}