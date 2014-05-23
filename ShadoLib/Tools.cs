using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Text.RegularExpressions;

namespace ShadoLib
{
    public static class Tools
    {
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

        public static Hashtable ArgCutter(String[] args)
        {
            var argsLine = String.Join(" ", args);

            var mc = Regex.Matches(argsLine, "-([a-z0-9]{1,9})(?: (?!-)([^ ]{1,900}))?");

            var argTab = new Hashtable();

            foreach (Match m in mc)
                argTab.Add(m.Groups[1].ToString(), m.Groups[2].ToString());

            return argTab;
        }

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

                    return dict.Contains(path);
                }
            }            
        }
        
    }
}

