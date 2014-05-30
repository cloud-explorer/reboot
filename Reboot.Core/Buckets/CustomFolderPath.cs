using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using Sitecore.Buckets.Util;
using Sitecore.Data;
using Sitecore.Diagnostics;

namespace Projects.Reboot.Core.Buckets
{
    public class CustomFolderPath : IDynamicBucketFolderPath
    {
        /// <summary>
        ///     Getting the folder path by the first 3 characters of the item name
        /// </summary>
        
        public string GetFolderPath(Database database, string name, ID templateId, ID newItemId, ID parentItemId,
                                    DateTime creationDateOfNewItem)
        {
            Assert.ArgumentNotNull(newItemId, "newItemId");
            Assert.ArgumentNotNullOrEmpty(name, "name");
            string n = name.Trim()
                        .Replace(" ", string.Empty)
                        .Replace("_", string.Empty)
                        .Replace("-", string.Empty);
            if (parentItemId.Equals(RebootConstants.ModelRootId) &&
                templateId.Equals(RebootConstants.ModelTemplateId))
            {
                return GetModelFolderStructure(name);
            }
            return GetCustomFolderPath(name, newItemId, n);
        }

        private string GetCustomFolderPath(string name, ID newItemId, string n)
        {
            n = n.Replace(".", string.Empty);
            bool isNameAlphaNumeric = IsEnglishAlphabet(name);
            List<char> chars = new List<char>();
            char[] path;
            if (isNameAlphaNumeric && n.Length > 2)
            {
                path = n.ToCharArray(0, 3);
            }
            else
            {
                path = IdHelper.NormalizeGuid(newItemId)
                               .ToString(CultureInfo.InvariantCulture)
                               .Substring(0, 3)
                               .ToCharArray();
            }
            chars.AddRange(path);
            return string.Join(Constants.ContentPathSeperator, chars.ToArray()).ToLower();
        }

        private string GetModelFolderStructure(string name)
        {
            string[] nameParts = name.Split('_');
            StringBuilder output = new StringBuilder();
            // We do not want the name of the type as part of out folder structure
            for (int i = 0; i < nameParts.Length - 1; i++)
            {
                if (output.Length > 0) output.Append(Constants.ContentPathSeperator);
                output.Append(nameParts[i]);
            }
            string typeList = output.ToString();
            return typeList;
        }

        private  Boolean IsEnglishAlphabet(string strToCheck)
        {
            Regex rg = new Regex("[\x00-\x80]+");

            //if has non Alpa char, return false, else return true.
            return rg.IsMatch(strToCheck);
        }
    }
}