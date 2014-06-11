using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Castle.Core.Internal;
using Castle.MicroKernel.ModelBuilder.Descriptors;
using Glass.Mapper.Sc.Configuration.Attributes;
using Projects.Common.Utils;
using Projects.Models.Glass;
using Projects.Models.Glass.Common;
using Projects.Models.Glass.Reboot.Items;
using Projects.Models.Glass.Reboot.RenderingParameters;
using Projects.Reboot.Common;
using Projects.Reboot.Contracts;
using Sitecore.ContentSearch.Linq;
using Sitecore.ContentSearch.Utilities;
using Sitecore.ContentSearch.Linq.Utilities;
using Sitecore.Diagnostics;
using Sitecore.Mvc.Extensions;

namespace Projects.Website.Controllers
{
    public class SearchController : BaseController
    {
        private readonly ISiteSearchService _siteSearchService;

        public SearchController(ISiteSearchService siteSearchService)
        {
            _siteSearchService = siteSearchService;
        }

        public SearchController()
        {
            _siteSearchService = new ServiceFactory().GetService<ISiteSearchService>();
        }

        public ActionResult Facet()
        {
            var parameters = GetRenderingParameters<FacetingParameters>();
            var predicate = PredicateBuilder.True<FacetableContent>();
            foreach (Guid facet in parameters.TemplatesToFacets)
            {
                var temp = facet;
                predicate = predicate.Or(w => w.TemplateId == temp);
            }

            var results = _siteSearchService.GetFacetResultsAs<FacetableContent>(
                //Where Conditions
                w =>
                {
                    w = w.Where(predicate);
                    return w;
                }
                //Facet Set up
                , f =>
                {
                    //Expression<Func<FacetableContent, IGlassBase>> expression = o => o.Parent;
                    //f.FacetOn(expression);
                    //Expression<Func<FacetableContent, object>> keySelector = o => GenerateLambda<FacetableContent, Guid>(o, parameters.FacetBy, typeof(IEnumerable<>), "o");
                    //f = f.FacetOn(keySelector);
                    //f = f.FacetOn(o => o.Genres, 1)
                    //    .FacetOn(o => o.Popularity, 1)
                    //    .FacetOn(o => o.ReleaseDate, 1)
                    //    .FacetOn(o => o.Status, 1)
                    //    .FacetOn(o => o.VoteAverage, 1)
                    //    ;
                    Expression<Func<FacetableContent, object>> expression = o => (o.GetType().InvokeMember(parameters.FacetBy, BindingFlags.GetProperty, null, o, null));
                    f =f.FacetOn(expression);
                    return f;
                }
                //Sort order setup
                , s => s
                );
            return null;
        }

        //private static Func<IFacetableContent, IEnumerable<Guid>> GetSomething(IFacetableContent content, string propertyName, string parameterName)
        //{
        //    ParameterExpression parameter = Expression.Parameter(content.GetType(), parameterName);
        //    MemberExpression property = Expression.Property(parameter, propertyName);
        //    var queryableType = typeof(IEnumerable<>).MakeGenericType(typeof(Guid));
        //    var delegateType = typeof(Func<,>).MakeGenericType(typeof(IFacetableContent), queryableType);
        //    var expression = Expression.Lambda<Func<IFacetableContent, IEnumerable<Guid>>>(property, parameter);
        //   // Log.Info(string.Format("this expression is /r/n {0}", expression), expression);
        //    return expression;
        //}

        //private static LambdaExpression GenerateLambda<T, TK>(T entity, string propertyName, Type t, string parameterName = "o") where T : class, IPageBase
        //{
        //    ParameterExpression instance = Expression.Parameter(entity.GetType(), parameterName);
        //    MemberExpression property = Expression.Property(instance, propertyName);

        //    var queryableType = t.MakeGenericType(typeof(TK));
        //    var delegateType = typeof(Func<,>).MakeGenericType(typeof(T), queryableType);

        //    //var expression = Expression.Lambda(property, instance);
        //    var expression = Expression.Lambda(delegateType, property, instance);
        //    Log.Info(string.Format("this expression is /r/n {0}", expression), expression);
        //    return expression;
        //}

        //private static LambdaExpression GenerateLambda(FacetableContent entity, string propertyName, string parameterName = "o")
        //{
        //    ParameterExpression instance = Expression.Parameter(entity.GetType(), parameterName);
        //    MemberExpression property = Expression.Property(instance, propertyName);
        //    var queryableType = typeof(IEnumerable<>).MakeGenericType(typeof(Guid));
        //    var delegateType = typeof (Func<,>).MakeGenericType(typeof (FacetableContent), queryableType);
        //    var expression = Expression.Lambda(delegateType, property, instance);
        //    Log.Info(string.Format("this expression is /r/n {0}", expression), expression);
        //    return expression;
        //}
    }
}