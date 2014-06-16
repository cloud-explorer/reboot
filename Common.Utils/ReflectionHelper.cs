using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Glass.Mapper.Sc.Configuration.Attributes;
using Sitecore.Data.Items;

namespace Projects.Common.Core
{
    public static class ReflectionHelper
    {
        private static readonly Dictionary<string, Type[]> AssemblyTypes = new Dictionary<string, Type[]>();  
       
        public static bool HasMethod(this object objectToCheck, string methodName)
        {
            if (objectToCheck == null) return false;
            var type = objectToCheck.GetType();
            return type.GetMethod(methodName) != null;
        }

        public static bool HasProperty(this object objectToCheck, string propertyName)
        {
            if (objectToCheck == null) return false;
            var type = objectToCheck.GetType();
            return type.GetProperty(propertyName) != null;
        }

        public static void SetPropertyValue<T>(this T target, Expression<Func<T, object>> memberLamda, object value)
        {
            var memberSelectorExpression = memberLamda.Body as MemberExpression;
            if (memberSelectorExpression == null) return;
            var property = memberSelectorExpression.Member as PropertyInfo;
            if (property != null)
            {
                property.SetValue(target, value, null);
            }
        }

        public static Type GetTypeWithAttributeValue<TAttribute>(this Assembly assembly, Predicate<TAttribute> pred, bool onlyConcreteTypes = true)
        {

            Type[] types;
            if (AssemblyTypes.ContainsKey(assembly.ToString()))
            {
                types = AssemblyTypes[assembly.ToString()];
            }
            else
            {
                types = assembly.GetTypes();
                if (onlyConcreteTypes) types = types.Where(ty => !ty.IsInterface).ToArray();
                AssemblyTypes.Add(assembly.ToString(), types);
            }
            var t = types.FirstOrDefault(type => type.GetCustomAttributes(typeof (TAttribute), true)
                                                              .Cast<TAttribute>()
                                                              .Any(oTemp => pred(oTemp)));
            return t ?? typeof(Item);
        }

        public static IEnumerable<Type> GetAllTypesThatImplement<T>(List<string> assemblyNames)
        {
            IEnumerable<Assembly> loadAssemblies = assemblyNames.Select(a => Assembly.Load(new AssemblyName(a)));
            List<Type> types = loadAssemblies.SelectMany(assembly => assembly.GetTypes()
                                                            .Where(t => typeof (T).IsAssignableFrom(t)))
                                                            .ToList();


            return types;
        }

        public static T GetAttributeValue<T, TK>(TK obj) where T : class
        {
            return null;
        }

        public static Guid GetTemplateIdFromType(this Type t)
        {
            SitecoreTypeAttribute sitecoreTypeAttribute = (SitecoreTypeAttribute) t.GetCustomAttribute(typeof (SitecoreTypeAttribute), true);
            Guid templateId;
            if (Guid.TryParse(sitecoreTypeAttribute.TemplateId, out templateId))
            {
                return templateId;
            }
            return Guid.Empty;
        }

        //public static object GetPropertyWithFieldId(this Type t, Guid id)
        //{
        //    var props = t.GetProperties().Where(
        //        prop => Attribute.IsDefined(prop, typeof(SitecoreInfoAttribute)));
        //    foreach (PropertyInfo pi in props)
        //    {
        //        pi.
        //    }
        //}

        public static PropertyInfo GetPropertyFromFieldId<T>(Guid fieldId)
        {
            //Dictionary<string, string> _dict = new Dictionary<string, string>();

            PropertyInfo[] props = typeof(T).GetProperties();
            foreach (PropertyInfo prop in props)
            {
                object[] attrs = prop.GetCustomAttributes(true);
                if ((attrs.OfType<SitecoreFieldAttribute>()
                    .Select(sitecoreInfoAttr => new {sitecoreInfoAttr, propName = prop.Name})
                    .Select(@t => new Guid(@t.sitecoreInfoAttr.FieldId))).Any(val => val.Equals(fieldId)))
                {
                    return prop;
                }
            }

            return null;
        }
    }
}
