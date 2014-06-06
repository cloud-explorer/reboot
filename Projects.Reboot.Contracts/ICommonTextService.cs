using Projects.Common.Contracts;
using Sitecore.Globalization;

namespace Projects.Reboot.Contracts
{
    public interface ICommonTextService : IServiceContract
    {
        string GetTextFor(string itemName, Language language = null);
    }
}