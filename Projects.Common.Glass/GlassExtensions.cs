using System;
using System.Collections.Generic;
using System.Linq;
using Glass.Mapper.Sc.Fields;
using Projects.Models.Glass;
using Projects.Models.Glass.Common;
using Projects.Models.Glass.Common.FieldSections;

namespace Projects.Common.Glass
{
    public static class GlassExtensions
    {
        public static Link GetInternalLink<T>(this T item) where T : class, IGlassBase, IHasTitle
        {
            return GetInternalLink<T>(item, "", "_self");
        }

        public static Link GetInternalLink<T>(this T item, string cssClass="", string target="_self") where T : class, IGlassBase, IHasTitle
        {
            if(item == null) return null;
            Link linkField = new Link
                {
                    TargetId = item.Id,
                    Text = item.Title,
                    Title = item.Title,
                    Type = LinkType.Internal,
                    Target = target,
                    Url = item.Url
                };
            if (!string.IsNullOrEmpty(cssClass)) linkField.Class = cssClass;
            return linkField;
        }

        public static Link GetInternalLinkForBreadcrumb<T>(this T item, string cssClass = "", string target = "_self")
            where T : class, IGlassBase, IHasTitle, IHasBreadcrumb
        {
            Link internalLink = item.GetInternalLink();
            internalLink.Title = item.BreadcrumbTitle;
            internalLink.Text = (!string.IsNullOrEmpty(item.CssClassName)) 
                                ? string.Format("<i class='{0}'></i> {1}", item.CssClassName, item.BreadcrumbTitle)
                                : item.BreadcrumbTitle;
            return internalLink;
        }

        public static Image GetImage(Guid id, string title) 
        {
            Image imageField = new Image {MediaId = id, Alt = title, Title = title};
            return imageField;
        }

        public static bool IsCurrentItem<T>(this T item) where T : class, IGlassBase
        {
            T currentItem = item.Context.GetCurrentItem<T>();
            return item.Id.Equals(currentItem.Id);
        }

        public static IEnumerable<T> GetAncestors<T>(this T item) where T : class, IPageBase
        {
            var home = item.Context.GetHomeItem<IPageBase>();
            List<T> ancestors = new List<T>();
            while (!item.Id.Equals(home.Id) && item.Parent != null)
            {
                item = item.Context.GetItem<T>(item.Parent.Id, inferType: true);
                ancestors.Add(item);
            }
            ancestors.Reverse();
            return ancestors;
        }

        public static IEnumerable<T> GetNavigableAncestors<T>(this T item) where T : class, IPageBase
        {
            IEnumerable<T> ancestors = item.GetAncestors<T>();
            IEnumerable<T> navigableAncestors = ancestors.Where(a => !a.TemplateName.Equals("Bucket", StringComparison.CurrentCultureIgnoreCase))
                                                        .Select(a => a);
            return navigableAncestors;
        }

        public static T GetLinkedItem<T, TK>(this TK item, Link link) where T : class, IGlassBase, new()
            where TK : class, IGlassBase
        {
            if (link == null || link.TargetId.Equals(Guid.Empty)) return null;
            T linkedItem = item.Context.GetItem<T>(link.TargetId);
            return linkedItem;
        }
    }
}


