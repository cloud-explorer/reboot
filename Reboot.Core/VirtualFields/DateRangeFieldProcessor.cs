#region

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Linq.Common;
using Sitecore.ContentSearch.Pipelines.GetFacets;
using Sitecore.ContentSearch.Pipelines.ProcessFacets;
using Sitecore.ContentSearch.Utilities;

#endregion

namespace Projects.Reboot.Core.VirtualFields
{
    /// <summary>
    /// Based on http://www.sitecore.net/Community/Technical-Blogs/Sitecore-7-Development-Team/Posts/2013/05/Sitecore-7-Making-Google-Part-2.aspx
    /// </summary>
    public class DateRangeFieldProcessor : IVirtualFieldProcessor
    {
        #region IVirtualFieldProcessor Members

        public string FieldName
        {
            get { return "daterangehourresolution"; }
        }

        public TranslatedFieldQuery TranslateFieldQuery(string fieldName, object fieldValue, ComparisonType comparison,
            FieldNameTranslator fieldNameTranslator)
        {
            if (comparison == ComparisonType.OrderBy)
            {
                throw new InvalidOperationException(string.Format("Sorting by virtual field {0} is not supported.",
                    fieldName));
            }

            var translated = new TranslatedFieldQuery();

            switch (fieldValue.ToString())
            {
                case "lasthour":
                    translated.FieldComparisons.Add(
                        new Tuple<string, object, ComparisonType>(
                            fieldNameTranslator.GetIndexFieldName("daterange_hour", typeof (string)),
                            DateTime.Now.ToString("yyyyMMddHH"), ComparisonType.Equal));
                    return translated;

                case "last24":
                    translated.FieldComparisons.Add(
                        new Tuple<string, object, ComparisonType>(
                            fieldNameTranslator.GetIndexFieldName("__smallcreateddate", typeof (DateTime)),
                            DateTime.Today.ToString(ContentSearchConfigurationSettings.IndexDateFormat),
                            ComparisonType.Equal));
                    return translated;

                case "yesterday":
                    translated.FieldComparisons.Add(
                        new Tuple<string, object, ComparisonType>(
                            fieldNameTranslator.GetIndexFieldName("__smallcreateddate", typeof (DateTime)),
                            DateTime.Today.AddDays(-1).ToString(ContentSearchConfigurationSettings.IndexDateFormat),
                            ComparisonType.Equal));
                    return translated;

                case "thisweek":
                    translated.FieldComparisons.Add(
                        new Tuple<string, object, ComparisonType>(
                            fieldNameTranslator.GetIndexFieldName("daterange_week", typeof (string)),
                            DateTime.Now.ToString("yyyyMM") +
                            CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstDay,
                                DayOfWeek.Monday), ComparisonType.Equal));
                    return translated;

                case "lastweek":
                    translated.FieldComparisons.Add(
                        new Tuple<string, object, ComparisonType>(
                            fieldNameTranslator.GetIndexFieldName("daterange_week", typeof (string)),
                            DateTime.Now.AddDays(-7).ToString("yyyyMM") +
                            CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Now.AddDays(-7),
                                CalendarWeekRule.FirstDay, DayOfWeek.Monday), ComparisonType.Equal));
                    return translated;

                case "thismonth":
                    translated.FieldComparisons.Add(
                        new Tuple<string, object, ComparisonType>(
                            fieldNameTranslator.GetIndexFieldName("daterange_month", typeof (string)),
                            DateTime.Now.ToString("yyyyMM"), ComparisonType.Equal));
                    return translated;

                case "lastmonth":
                    translated.FieldComparisons.Add(
                        new Tuple<string, object, ComparisonType>(
                            fieldNameTranslator.GetIndexFieldName("daterange_month", typeof (string)),
                            DateTime.Now.AddMonths(-1).ToString("yyyyMM"), ComparisonType.Equal));
                    return translated;

                case "thisyear":
                    translated.FieldComparisons.Add(
                        new Tuple<string, object, ComparisonType>(
                            fieldNameTranslator.GetIndexFieldName("daterange_year", typeof (string)),
                            DateTime.Now.ToString("yyyy"), ComparisonType.Equal));
                    return translated;

                case "older":
                    translated.FieldComparisons.Add(
                        new Tuple<string, object, ComparisonType>(
                            fieldNameTranslator.GetIndexFieldName("__smallcreateddate", typeof (DateTime)),
                            DateTime.Now.AddDays(DateTime.Now.Day).AddMonths(-1), ComparisonType.LessThan));
                    return translated;
            }

            translated.FieldComparisons.Add(new Tuple<string, object, ComparisonType>(fieldName, fieldValue,
                ComparisonType.Equal));
            return translated;
        }

        public IDictionary<string, object> TranslateFieldResult(IDictionary<string, object> fields,
            FieldNameTranslator fieldNameTranslator)
        {
            var smallCreated = fieldNameTranslator.GetIndexFieldName("__smallcreateddate", typeof (DateTime));
            var dateRangeHour = fieldNameTranslator.GetIndexFieldName("daterange_hour", typeof (string));
            var dateRangeWeek = fieldNameTranslator.GetIndexFieldName("daterange_week", typeof (string));
            var dateRangeMonth = fieldNameTranslator.GetIndexFieldName("daterange_month", typeof (string));
            var dateRangeYear = fieldNameTranslator.GetIndexFieldName("daterange_year", typeof (string));

            if (fields.ContainsKey(dateRangeHour) &&
                fields[dateRangeHour].ToString() == DateTime.Now.ToString("yyyyMMddHH"))
            {
                fields[FieldName] = "lasthour";
            }
            else if (fields.ContainsKey(smallCreated) &&
                     fields[smallCreated].ToString() ==
                     DateTime.Now.Date.ToString(ContentSearchConfigurationSettings.IndexDateFormat))
            {
                fields[FieldName] = "last24";
            }
            else if (fields.ContainsKey(smallCreated) &&
                     fields[smallCreated].ToString() ==
                     DateTime.Now.Date.AddDays(-1).ToString(ContentSearchConfigurationSettings.IndexDateFormat))
            {
                fields[FieldName] = "yesterday";
            }
            else if (fields.ContainsKey(dateRangeWeek) &&
                     fields[dateRangeWeek].ToString() ==
                     DateTime.Now.ToString("yyyyMM") +
                     CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstDay,
                         DayOfWeek.Monday))
            {
                fields[FieldName] = "thisweek";
            }
            else if (fields.ContainsKey(dateRangeWeek) &&
                     fields[dateRangeWeek].ToString() ==
                     DateTime.Now.AddDays(-7).ToString("yyyyMM") +
                     CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Now.AddDays(-7),
                         CalendarWeekRule.FirstDay, DayOfWeek.Monday))
            {
                fields[FieldName] = "lastweek";
            }
            else if (fields.ContainsKey(dateRangeMonth) &&
                     fields[dateRangeMonth].ToString() == DateTime.Now.ToString("yyyyMM"))
            {
                fields[FieldName] = "thismonth";
            }
            else if (fields.ContainsKey(dateRangeMonth) &&
                     fields[dateRangeMonth].ToString() ==
                     DateTime.Now.AddMonths(-1).ToString("yyyyMM"))
            {
                fields[FieldName] = "lastmonth";
            }
            else if (fields.ContainsKey(dateRangeYear) &&
                     fields[dateRangeYear].ToString() == DateTime.Now.ToString("yyyy"))
            {
                fields[FieldName] = "thisyear";
            }
            else
            {
                fields[FieldName] = "older";
            }

            return fields;
        }

        public GetFacetsArgs TranslateFacetQuery(GetFacetsArgs args)
        {
            var facetQueries = args.FacetQueries.ToList();

            var daterangeFacet =
                facetQueries.Where(q => q.FieldNames.Count() == 1 && q.FieldNames.First() == "daterangehourresolution")
                    .ToList();
            if (daterangeFacet.Count > 0)
            {
                int? min = daterangeFacet[0].MinimumResultCount;

                facetQueries.Add(new FacetQuery(null,
                    new[] {args.FieldNameTranslator.GetIndexFieldName("daterange_hour", typeof (string))}, min,
                    new[] {DateTime.Now.ToString("yyyyMMddHH")}));
                facetQueries.Add(new FacetQuery(null,
                    new[] {args.FieldNameTranslator.GetIndexFieldName("__smallcreateddate", typeof (DateTime))}, min,
                    new[]
                    {
                        DateTime.Now.Date.ToString(ContentSearchConfigurationSettings.IndexDateFormat),
                        DateTime.Now.AddDays(-1).Date.ToString(ContentSearchConfigurationSettings.IndexDateFormat)
                    }));
                facetQueries.Add(new FacetQuery(null,
                    new[] {args.FieldNameTranslator.GetIndexFieldName("daterange_week", typeof (string))}, min,
                    new[]
                    {
                        DateTime.Now.ToString("yyyyMM") +
                        CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstDay,
                            DayOfWeek.Monday)
                    }));
                facetQueries.Add(new FacetQuery(null,
                    new[] {args.FieldNameTranslator.GetIndexFieldName("daterange_month", typeof (string))}, min,
                    new[] {DateTime.Now.ToString("yyyyMM")}));
                facetQueries.Add(new FacetQuery(null,
                    new[] {args.FieldNameTranslator.GetIndexFieldName("daterange_year", typeof (string))}, min, null));

                daterangeFacet.ForEach(f => facetQueries.Remove(f));
            }

            return new GetFacetsArgs(args.BaseQuery, facetQueries, args.VirtualFieldProcessors, args.FieldNameTranslator);
        }

        public IDictionary<string, ICollection<KeyValuePair<string, int>>> TranslateFacetResult(ProcessFacetsArgs args)
        {
            var facets = args.Facets;

            var daterangeFacet =
                args.OriginalFacetQueries.Where(
                    q => q.FieldNames.Count() == 1 && q.FieldNames.First() == "daterangehourresolution").ToList();

            if (daterangeFacet.Count > 0)
            {
                var dateRangeFacet = new List<KeyValuePair<string, int>>();

                ICollection<KeyValuePair<string, int>> hourFacet;
                ICollection<KeyValuePair<string, int>> dateFacet;
                ICollection<KeyValuePair<string, int>> weekFacet;
                ICollection<KeyValuePair<string, int>> monthFacet;
                ICollection<KeyValuePair<string, int>> yearFacet;

                facets.TryGetValue(args.FieldNameTranslator.GetIndexFieldName("daterange_hour", typeof (string)),
                    out hourFacet);
                facets.TryGetValue(args.FieldNameTranslator.GetIndexFieldName("__smallcreateddate", typeof (DateTime)),
                    out dateFacet);
                facets.TryGetValue(args.FieldNameTranslator.GetIndexFieldName("daterange_week", typeof (string)),
                    out weekFacet);
                facets.TryGetValue(args.FieldNameTranslator.GetIndexFieldName("daterange_month", typeof (string)),
                    out monthFacet);
                facets.TryGetValue(args.FieldNameTranslator.GetIndexFieldName("daterange_year", typeof (string)),
                    out yearFacet);

                if (hourFacet != null)
                {
                    var hour = hourFacet.FirstOrDefault(v => v.Key == DateTime.Now.ToString("yyyyMMddHH"));

                    if (hour.Key != null)
                        dateRangeFacet.Add(new KeyValuePair<string, int>("lasthour", hour.Value));

                    var lastmonth =
                        hourFacet.FirstOrDefault(v => v.Key == DateTime.Now.AddHours(-1).ToString("yyyyMMddHH"));

                    if (lastmonth.Key != null)
                        dateRangeFacet.Add(new KeyValuePair<string, int>("lasthour", lastmonth.Value));
                }

                if (dateFacet != null)
                {
                    var today =
                        dateFacet.FirstOrDefault(
                            v => v.Key == DateTime.Today.ToString(ContentSearchConfigurationSettings.IndexDateFormat));

                    if (today.Key != null)
                        dateRangeFacet.Add(new KeyValuePair<string, int>("today", today.Value));

                    var yesterday =
                        dateFacet.FirstOrDefault(
                            v =>
                                v.Key ==
                                DateTime.Today.AddDays(-1).ToString(ContentSearchConfigurationSettings.IndexDateFormat));

                    if (yesterday.Key != null)
                        dateRangeFacet.Add(new KeyValuePair<string, int>("yesterday", yesterday.Value));
                }

                if (weekFacet != null)
                {
                    var week =
                        weekFacet.FirstOrDefault(
                            v =>
                                v.Key ==
                                DateTime.Now.ToString("yyyyMM") +
                                CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Now,
                                    CalendarWeekRule.FirstDay, DayOfWeek.Monday));

                    if (week.Key != null)
                        dateRangeFacet.Add(new KeyValuePair<string, int>("thisweek", week.Value));

                    var lastweek =
                        weekFacet.FirstOrDefault(
                            v =>
                                v.Key ==
                                DateTime.Now.AddDays(-7).ToString("yyyyMM") +
                                CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Now.AddDays(-7),
                                    CalendarWeekRule.FirstDay, DayOfWeek.Monday));

                    if (lastweek.Key != null)
                        dateRangeFacet.Add(new KeyValuePair<string, int>("lastweek", lastweek.Value));
                }

                if (monthFacet != null)
                {
                    var month = monthFacet.FirstOrDefault(v => v.Key == DateTime.Now.ToString("yyyyMM"));

                    if (month.Key != null)
                        dateRangeFacet.Add(new KeyValuePair<string, int>("thismonth", month.Value));

                    var lastmonth =
                        monthFacet.FirstOrDefault(v => v.Key == DateTime.Now.AddMonths(-1).ToString("yyyyMM"));

                    if (lastmonth.Key != null)
                        dateRangeFacet.Add(new KeyValuePair<string, int>("lastmonth", lastmonth.Value));
                }

                int thisYearCount = 0;

                if (yearFacet != null)
                {
                    var year = yearFacet.FirstOrDefault(v => v.Key == DateTime.Now.ToString("yyyy"));

                    if (year.Key != null)
                    {
                        dateRangeFacet.Add(new KeyValuePair<string, int>("thisyear", year.Value));
                        thisYearCount = year.Value;
                    }

                    var calculatedFacetCount = yearFacet.Sum(f => f.Value) - thisYearCount;
                    if (calculatedFacetCount >= daterangeFacet.First().MinimumResultCount)
                    {
                        dateRangeFacet.Add(new KeyValuePair<string, int>("older", calculatedFacetCount));
                    }
                }

                facets["daterange"] = dateRangeFacet;
                facets.Remove(args.FieldNameTranslator.GetIndexFieldName("daterange_hour"));
                facets.Remove(args.FieldNameTranslator.GetIndexFieldName("__smallcreateddate"));
                facets.Remove(args.FieldNameTranslator.GetIndexFieldName("daterange_week"));
                facets.Remove(args.FieldNameTranslator.GetIndexFieldName("daterange_month"));
                facets.Remove(args.FieldNameTranslator.GetIndexFieldName("daterange_year"));
            }

            return facets;
        }

        #endregion
    }
}