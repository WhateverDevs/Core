using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace WhateverDevs.Core.Runtime.DataStructures.Integration
{
    /// <summary>
    /// A Simple CSV reader based on this:
    /// https://github.com/tikonen/blog/tree/master/csvreader
    /// Altered it from reading a TextAsset into parsing a CSV string.
    /// </summary>
    public static class CsvReader
    {
        private const string LineSplitRe = @"\r\n|\n\r|\n|\r";
        private static readonly char[] TrimChars = { '\"' };

        /// <summary>
        /// Function that reads each column of a CSV string and creates a dictionary for each of those columns.
        /// </summary>
        public static List<Dictionary<string, string>> ReadColumnsFromCsv(string file, string separator)
        {
            List<Dictionary<string, string>> list = new();

            string[] lines = Regex.Split(file, LineSplitRe);

            if (lines.Length <= 1) return list;

            string[] header = Regex.Split(lines[0], separator);

            for (int i = 1; i < lines.Length; ++i)
            {
                string[] values = Regex.Split(lines[i], separator);
                if (values.Length == 0 || values[0] == "") continue;

                Dictionary<string, string> entry = new();

                for (int j = 0; j < header.Length && j < values.Length; ++j)
                {
                    string value = values[j];
                    value = value.TrimStart(TrimChars).TrimEnd(TrimChars).Replace("\\", "");
                    entry[header[j]] = value;
                }

                list.Add(entry);
            }

            return list;
        }
    }
}