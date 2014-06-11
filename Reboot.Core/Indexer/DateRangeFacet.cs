using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;

namespace Projects.Reboot.Core.Indexer
{
    public abstract class DateRangeFacet : IComputedIndexField
    {

        public string FieldName { get; set; }
        public string ReturnType { get; set; }

        public object ComputeFieldValue(IIndexable indexable)
        {
            Item item = indexable as SitecoreIndexableItem;

            var dimension = new List<string>();
            //This field is the field you will soon create that stores the created date in the index with Hour Resolution i.e. 2013-01-01T01-01 but we will store it like this  201301010101
            var field = item.Fields["__createdtohourresolution"];
            if (field != null)
            {
                if (!string.IsNullOrEmpty(field.Value))
                {
                    var dateField = ((DateField)field).DateTime;

                    var size = this.GetDateRange(dateField);

                    dimension.Add(size);
                    return dimension;
                }
            }
            else
            {
                //default fallback value for range grouping
                dimension.Add("older");
                return dimension;
            }

            return dimension;
        }

        protected abstract string GetDateRange(DateTime dateTime);
    }

    public class DateRangeMonthFacet : DateRangeFacet
    {
        protected override string GetDateRange(DateTime dateTime)
        {
            return dateTime.ToString("yyyyMM");
        }
    }

    public class DateRangeWeekFacet : DateRangeFacet
    {
        protected override string GetDateRange(DateTime dateTime)
        {
            return dateTime.ToString("yyyyMM") + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(dateTime, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
        }
    }

    public class DateRangeYearFacet : DateRangeFacet
    {
        protected override string GetDateRange(DateTime dateTime)
        {
            return dateTime.ToString("yyyy");
        }
    }

    public class DateRangeHourFacet : DateRangeFacet
    {
        protected override string GetDateRange(DateTime dateTime)
        {
            return dateTime.ToString("yyyyMMddHH");
        }
    }
}
