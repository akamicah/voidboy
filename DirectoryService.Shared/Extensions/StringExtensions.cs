using System.Text.RegularExpressions;

namespace DirectoryService.Shared.Extensions;

public static class StringExtensions
{
        public static string MaskEmail(this string s)
        {
            const string pattern = @"(?<=[\w]{1})[\w-\._\+%\\]*(?=[\w]{1}@)|(?<=@[\w]{1})[\w-_\+%]*(?=\.)";
            
            if (!s.Contains('@'))
                return new string('*', s.Length);
            
            return s.Split('@')[0].Length < 4 ? @"*@*.*" : 
                Regex.Replace(s, pattern, m => new string('*', m.Length));
        }
}