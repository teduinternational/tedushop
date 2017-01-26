using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TeduShop.Common
{
    public static class StringHelper
    {
        public static string ToUnsignString(string input)
        {
            input = input.Trim();
            for (int i = 0x20; i < 0x30; i++)
            {
                input = input.Replace(((char)i).ToString(), " ");
            }
            input = input.Replace(".", "-");
            input = input.Replace(" ", "-");
            input = input.Replace(",", "-");
            input = input.Replace(";", "-");
            input = input.Replace(":", "-");
            input = input.Replace("  ", "-");
            Regex regex = new Regex(@"\p{IsCombiningDiacriticalMarks}+");
            string str = input.Normalize(NormalizationForm.FormD);
            string str2 = regex.Replace(str, string.Empty).Replace('đ', 'd').Replace('Đ', 'D');
            while (str2.IndexOf("?") >= 0)
            {
                str2 = str2.Remove(str2.IndexOf("?"), 1);
            }
            while (str2.Contains("--"))
            {
                str2 = str2.Replace("--", "-").ToLower();
            }
            return str2;
        }
        /// <summary>
        /// Replace HTML template with values
        /// </summary>
        /// <param name="template">Template content HTML</param>
        /// <param name="replacements">Dictionary with key/value</param>
        /// <returns></returns>
        public static string Parse(this string template, Dictionary<string, string> replacements)
        {
            if (replacements.Count > 0)
            {
                template = replacements.Keys
                            .Aggregate(template, (current, key) => current.Replace(key, replacements[key]));
            }
            return template;
        }
    }
}
