using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Projects.Models.Glass.Common;
using Projects.Reboot.Common;
using Projects.Reboot.Contracts;
using Sitecore.Globalization;

namespace Projects.Reboot.Core
{
    public class CommonTextUtil
    {
        static readonly ICommonTextService  TextService;
        

        static CommonTextUtil()
        {
            TextService = new ServiceFactory().GetService<ICommonTextService>();
        }

        public static string GetTextFor(string itemName, Language language = null)
        {
            return TextService.GetTextFor(itemName, language);
        }

        public static StandardText GetItemFor(string itemName, Language language = null)
        {
            return TextService.GetItemFor(itemName, language);
        }
    }
}
