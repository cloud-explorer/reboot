using System;
using System.IO;
using System.Net;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using TMDbLib.Client;

namespace Projects.Reboot.DataMigration.Utils
{
    class MediaExtensions
    {
        public static MediaItem AddImage(Stream stream, string sitecorePath, string mediaItemName)
        {
            Sitecore.Resources.Media.MediaCreatorOptions options = new Sitecore.Resources.Media.MediaCreatorOptions
                {
                    FileBased = false,
                    IncludeExtensionInItemName = false,
                    KeepExisting = false,
                    Versioned = false,
                    Destination = sitecorePath + "/" + mediaItemName,
                    Database = Sitecore.Configuration.Factory.GetDatabase("master"),
                    AlternateText = mediaItemName
                };
            
            Item mediaItem = Sitecore.Resources.Media.MediaManager.Creator.AttachStreamToMediaItem(stream, sitecorePath,
                                                                                                   mediaItemName, options);
            return mediaItem;
        }

        public static MediaItem AddFile(string fileName, string sitecorePath, string mediaItemName)
        {
            // Create the options
            Sitecore.Resources.Media.MediaCreatorOptions options = new Sitecore.Resources.Media.MediaCreatorOptions();
            // Store the file in the database, not as a file
            options.FileBased = false;
            // Remove file extension from item name
            options.IncludeExtensionInItemName = false;
            // Overwrite any existing file with the same name
            options.KeepExisting = false;
            // Do not make a versioned template
            options.Versioned = false;
            // set the path
            options.Destination = sitecorePath + "/" + ItemUtil.ProposeValidItemName(mediaItemName);
            // Set the database
            options.Database = Sitecore.Configuration.Factory.GetDatabase("master");

            // Now create the file
            Sitecore.Resources.Media.MediaCreator creator = new Sitecore.Resources.Media.MediaCreator();
            MediaItem mediaItem = creator.CreateFromFile(fileName, options);
            return mediaItem;
        }

        public static Guid AddMovieDBImage(TMDbClient client, string fileName, string size = "original")
        {
            ID id = IDTableExtesions.GetItemIdFromIDTableEntry(RebootConstants.ImageItemPrefix, fileName);
            if (!id.IsNull) return id.Guid;
            Uri imageUrl = client.GetImageUrl(size, fileName);
            string imageTempPath = System.IO.Path.GetTempPath() + fileName.Trim('/');
            try
            {
                DownloadRemoteImageFile(imageUrl.ToString(), imageTempPath);
                Item mediaItem = AddFile(imageTempPath, RebootConstants.ImageRootPath, fileName.Trim('/'));
                if (mediaItem == null) return Guid.Empty;
                //IDTableExtesions.AddIDTableEntry("Image", fileName, mediaItem.ID, RebootConstants.ImageRootID, fileName);
                return mediaItem.ID.Guid;
            }
            catch (Exception ex)
            {
                Log.Error("Error while uploading image to media library. URL is " + imageTempPath, ex, client);
                return Guid.Empty;
            }
            finally
            {
                if(File.Exists(imageTempPath)) File.Delete(imageTempPath);
            }
        }

        private static void DownloadRemoteImageFile(string uri, string fileName)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            // Check that the remote file was found. The ContentType
            // check is performed since a request for a non-existent
            // image file might be redirected to a 404-page, which would
            // yield the StatusCode "OK", even though the image was not
            // found.
            if ((response.StatusCode == HttpStatusCode.OK ||
                response.StatusCode == HttpStatusCode.Moved ||
                response.StatusCode == HttpStatusCode.Redirect) &&
                response.ContentType.StartsWith("image", StringComparison.OrdinalIgnoreCase))
            {

                // if the remote file was found, download oit
                using (Stream inputStream = response.GetResponseStream())
                using (Stream outputStream = File.OpenWrite(fileName))
                {
                    byte[] buffer = new byte[4096];
                    int bytesRead;
                    do
                    {
                        bytesRead = inputStream.Read(buffer, 0, buffer.Length);
                        outputStream.Write(buffer, 0, bytesRead);
                    } while (bytesRead != 0);
                }
            }
        }
    }
}
