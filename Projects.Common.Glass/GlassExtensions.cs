using System;
using Glass.Mapper.Sc.Fields;
using Projects.Models.Glass;
using Projects.Models.Glass.Common.FieldSections;

namespace Projects.Common.Glass
{
    public static class GlassExtensions
    {
        public static Link GetInternalLink<T>(this T item) where T : class, IGlassBase, IHasTitle
        {
            if(item == null) return null;
            Link linkField = new Link
                {
                    TargetId = item.Id,
                    Text = item.Title,
                    Title = item.Title,
                    Type = LinkType.Internal
                };
            return linkField;
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

    }
}
