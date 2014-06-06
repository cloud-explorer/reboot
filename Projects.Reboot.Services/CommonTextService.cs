using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Projects.Models.Glass;
using Projects.Models.Glass.Common;
using Projects.Reboot.Contracts;
using Sitecore.Data.Managers;
using Sitecore.Globalization;
using Sitecore.Rules.Conditions;

namespace Projects.Reboot.Services
{
    public class CommonTextService : SearchService, ICommonTextService
    {
        public string GetTextFor(string itemName, Language language = null)
        {
            if (language == null) language = LanguageManager.DefaultLanguage;
            IStandardText item;
            using (var context = Index.CreateSearchContext())
            {
                item = context.GetQueryable<IStandardText>().FirstOrDefault(m => m.Name == itemName
                                                                                 && m.Language == language);
            }
            if (item == null)
            {
                //If there is nothing found, the item name will be returned back
                return itemName; 
            }
            return item.Text;
        }

  
    }
}
