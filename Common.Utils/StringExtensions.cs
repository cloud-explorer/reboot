using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Projects.Common.Core
{
    public static class StringExtensions
    {
        /// <summary>
        ///     Gets the completed word with desired length.  Will keep moving to next letter to check.
        /// </summary>
        /// <param name="s"> This string. </param>
        /// <param name="length"> The minimum length. </param>
        /// <param name="atWord"> </param>
        /// <param name="addEllipsis"> </param>
        /// <returns> The resulting string. </returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref>
        ///         <name>delimiters</name>
        ///     </paramref>
        ///     was
        ///     <see langword="null" />
        ///     or empty.
        /// </exception>
        public static string Truncate(this string s, int length, bool atWord = true, bool addEllipsis = false)
        {
            // Return if the string is less than or equal to the truncation length
            if (s == null || s.Length <= length)
                return s;

            // Do a simple truncation at the desired length
            string s2 = s.Substring(0, length);

            // Truncate the string at the word
            if (atWord)
            {
                // List of characters that denote the start or a new word (add to or remove more as necessary)
                List<char> alternativeCutOffs = new List<char>
                    {
                        ' ',
                        ',',
                        '.',
                        '?',
                        '/',
                        ':',
                        ';',
                        '\'',
                        '\"',
                        '\'',
                        '-'
                    };

                // Get the index of the last space in the truncated string
                int lastSpace = s2.LastIndexOf(' ');

                // If the last space index isn't -1 and also the next character in the original
                // string isn't contained in the alternativeCutOffs List (which means the previous
                // truncation actually truncated at the end of a word),then shorten string to the last space
                if (lastSpace != -1 && (s.Length >= length + 1 && !alternativeCutOffs.Contains(s.ToCharArray()[length])))
                    s2 = s2.Remove(lastSpace);
            }

            // Add Ellipsis if desired
            if (addEllipsis)
                s2 += "...";

            return s2;
        }

        /// <summary>
        ///     Strips the HTML tags in the input string.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns></returns>
        public static string StripHTML(this string s)
        {
            if (String.IsNullOrEmpty(s))
                return s;

            string rv = s;

            Regex html = new Regex("<[^>]*(>|$)",
                                   RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);

            // replace the tags with a space so words don't get bunched up together
            rv = html.Replace(rv, " ");

            // remove any double spaces so it doesn't look funky
            while (rv.Contains("  "))
                rv = rv.Replace("  ", " ");

            return rv;
        }
    }
}
