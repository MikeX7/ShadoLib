using System;
using System.Collections;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace ShadoLib
{
    static public class Tools
    {
        public static String BytesFormat(ulong bytes)
        {           
            if (bytes >= 1125899906842624)
                return ((double)bytes / 1125899906842624).ToString("0.##") + "PB";
            else if (bytes >= 1099511627776)
                return ((double)bytes / 1099511627776).ToString("0.##") + "TB";
            else if (bytes >= 1073741824)
                return ((double)bytes / 1073741824).ToString("0.##") + "GB";
            else if (bytes >= 1048576)
                return ((double)bytes / 1048576).ToString("0.##") + "MB";
            else if (bytes >= 1024)
                return ((double)bytes / 1024).ToString("0.##") + "KB";
            else
                return bytes + "B";

            
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
    }
}

