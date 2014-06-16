using Projects.Common.Contracts;
using Projects.Models.Glass.Common;
using Sitecore.Globalization;

namespace Projects.Reboot.Contracts
{
    public interface ICommonTextService : IServiceContract
    {
        string GetTextFor(string itemName, Language language = null);
        StandardText GetItemFor(string itemName, Language language = null);
    }
}