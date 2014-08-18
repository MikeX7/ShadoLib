using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace ShadoLib
{
    public static class Tools
    {
        /// <summary>
        /// Formate a supplied number of bytes into a human friendly string.
        /// </summary>
        /// <param name="bytes">Number of bytes to format</param>
        /// <returns>Supplied bytes formated as a string, for example 1024 will return 1 KB</returns>
        public static String BytesFormat(ulong bytes)
        {
            if (bytes >= 1125899906842624)
                return ((double) bytes/1125899906842624).ToString("0.##") + " PB";
            else if (bytes >= 1099511627776)
                return ((double) bytes/1099511627776).ToString("0.##") + " TB";
            else if (bytes >= 1073741824)
                return ((double) bytes/1073741824).ToString("0.##") + " GB";
            else if (bytes >= 1048576)
                return ((double) bytes/1048576).ToString("0.#") + " MB";
            else if (bytes >= 1024)
                return ((double) bytes/1024).ToString("0") + " KB";
            else
                return bytes + " B";


        }

        /// <summary>
        /// Take arguments and their values (if any) supplied from a command line and reformat them into key-value pairs, saved into a hashtable
        /// </summary>
        /// <param name="args">Array of arguments usually obtained from the system</param>
        /// <returns>Array where names of the arguments are keys and their values are assigned to the keys</returns>
        public static Hashtable ArgCutter(String[] args)
        {
            var argsLine = String.Join(" ", args);

            var mc = Regex.Matches(argsLine, "-([a-z0-9]{1,9})(?: (?!-)([^ ]{1,900}))?");

            var argTab = new Hashtable();

            foreach (Match m in mc)
                argTab.Add(m.Groups[1].ToString(), m.Groups[2].ToString());

            return argTab;
        }

        /// <summary>
        /// Check if given resource exists in the specified assembly.
        /// </summary>
        /// <param name="path">Relative path to the resource</param>
        /// <param name="assembly">Assembly in which the presence of the resource should be checked</param>
        /// <returns>True if the resource exists, false if it does not</returns>
        public static bool ResourceExists(string path, Assembly assembly)
        {           
            using (var stream = assembly.GetManifestResourceStream(assembly.GetName().Name + ".g.resources"))
            {
                if (stream == null) 
                    return false;

                using (var reader = new System.Resources.ResourceReader(stream))
                {
                    var dict = reader.Cast<DictionaryEntry>().Select(entry =>
                        (string)entry.Key).ToArray();

                    return dict.Contains(path.ToLower());
                }
            }            
        }

        /// <summary>
        /// Recursively generate a list of all directories and subdirectories contained within the supplied root directory
        /// </summary>
        /// <param name="rootDir">Path to the root directory, from which the search begins</param>
        /// <returns>Flat list of all directories, including subdirectories, contained within the supplied root directory</returns>
        public static List<string> GetDirectoryList(string rootDir, List<string> dirList = null)
        {
            if(dirList == null)
                dirList = new List<string>();

            dirList.Add(rootDir);

            if (!Directory.Exists(rootDir))
                return dirList;

            var dirs = Directory.GetDirectories(rootDir);

            foreach (var dir in dirs)
            {
                dirList.Add(dir);
                dirList.AddRange(GetDirectoryList(dir, dirList));
            }

            return dirList.Distinct().ToList();
        }

        /// <summary>
        /// Recursively scan through the given folder and remove any empty directories within it, or its sub-folders.
        /// </summary>
        /// <param name="rootDir">Root directory, from which the scan starts</param>
        public static void RemoveEmptyDirectories(string rootDir)
        {                

            if (!Directory.Exists(rootDir))
                return;

            var dirs = Directory.GetDirectories(rootDir);

            foreach (var dir in dirs)
            {
                RemoveEmptyDirectories(dir);

                if (Directory.EnumerateFileSystemEntries(dir).Any()) 
                    continue;

                try
                {
                    Directory.Delete(dir);
                }
                catch (Exception){}
            }            
            
        }                
    }
}

