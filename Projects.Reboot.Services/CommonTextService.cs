using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glass.Mapper;
using Projects.Models.Glass;
using Projects.Models.Glass.Common;
using Projects.Reboot.Contracts;
using Projects.Reboot.Core;
using Sitecore.Data.Managers;
using Sitecore.Globalization;
using Sitecore.Rules.Conditions;

namespace Projects.Reboot.Services
{
    public class CommonTextService : SearchServiceBase, ICommonTextService
    {
        public string GetTextFor(string itemName, Language language = null)
        {
            if (language == null) language = LanguageManager.DefaultLanguage;
            IStandardText item;
            using (var context = Index.CreateSearchContext())
            {
                item = context.GetQueryable<StandardText>().FirstOrDefault(m => m.TemplateId == IStandardTextConstants.TemplateId.Guid
                                                                                  &&  m.Name == itemName
                                                                                 && m.Language == language);
            }
            if (item == null)
            {
                //If there is nothing found, the item name will be returned back
                return itemName; 
            }
            StandardText standardText = _context.GetItem<StandardText>(item.Id);
            return standardText.Text;
        }

  
    }
}
