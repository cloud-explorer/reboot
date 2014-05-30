using System;
using Projects.Reboot.DataMigration.Utils;
using TMDbLib.Client;
using TMDbLib.Objects.General;

namespace Projects.Reboot.DataMigration.Managers
{
    internal class ManagerBase
    {
        protected readonly TMDbClient _client = new TMDbClient(RebootConstants.ApiKey);

        public ManagerBase()
        {
            _client.GetConfig();
        }

        protected Guid AddImage(ImageData m)
        {
            Guid dbImageId = MediaExtensions.AddMovieDBImage(_client, m.FilePath);
            return dbImageId;
        }
    }
}
