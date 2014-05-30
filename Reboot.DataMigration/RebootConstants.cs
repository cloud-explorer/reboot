using Sitecore.Data;

namespace Projects.Reboot.DataMigration
{
    internal static class RebootConstants
    {
        internal static readonly ID MovieRootID = new ID("{64276281-ED5C-498C-BB1C-6448AC713815}");
        internal static readonly ID PeopleRootID = new ID("{D8C3A8C3-7977-4E65-8535-4B8D3ED02A7C}");
        internal static readonly ID GenresRootID = new ID("{36C9D2C6-D2A9-4CD5-9D10-095C9FA799EB}");
        internal static readonly ID LanguageRootID = new ID("{8BFBBF9D-678C-45EB-89A4-A22D6758624C}");
        internal static readonly ID CompanyRootId = new ID("{3939468F-0810-453C-AE49-C06561D44817}");
        internal static readonly ID TrailerRootID = new ID("{2ED74ED2-6E9D-4414-B22A-D435B33E557F}");
        internal static readonly ID ProductionCompaniesRootID = new ID("{3939468F-0810-453C-AE49-C06561D44817}");
        internal static readonly ID CastCrewRootID = new ID("{3F473DC1-8D54-4FEA-9746-B45C531FE16C}");
        internal static readonly string ApiKey = "39b851da4ab98d9742277a328cda7289";

        internal static readonly string GenrePrefix = "genre";
        internal static readonly string PeoplePrefix = "people";
        internal static readonly string LanguagePrefix = "language";
        internal static readonly string CompanyPrefix = "company";
        internal static readonly string TrailerPrefix = "trailer";
        internal static readonly string CastrPrefix = "cast";
        internal static readonly string CrewPrefix = "crew";
        internal const int PageSize = 20;
        internal const string ImageRootPath = "/sitecore/media library/Images/Reboot";
        internal const string ImageItemPrefix = "Reboot.Images";
        internal static readonly ID ImageRootID = new ID("{EFC7FE78-CCB7-46CD-9510-517FE32FE9CB}");
    }
}
