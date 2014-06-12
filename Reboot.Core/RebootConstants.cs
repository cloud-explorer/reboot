#region

using Sitecore.Data;

#endregion

namespace Projects.Reboot.Core
{
    public static class RebootConstants
    {
        #region Readonly & Static Fields

        public static ID ModelRootId = new ID("{B87CD5F0-4E72-429D-90A3-B285F1D038CA}");
        public static ID ModelTemplateId = new ID("{FED6A14F-0D05-4E18-B160-17C0588A2005}");
        public static ID CommonTextRootId = new ID("{3903A02E-42A5-4443-8974-5E2058552E28}");
        public const string CommonTextRootpath = "/sitecore/content/reboot/global/common text";
        #endregion
    }
}