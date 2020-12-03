using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace v10s_c3ns0r
{
    public class Censor : ICensor
    {
        private readonly IWordDictionary _wordDictionary;

        public Censor(IWordDictionary wordDictionary)
        {
            _wordDictionary = wordDictionary;
        }

        public bool HasMatch(string phrase)
        {
            var testableString = phrase.ToLowerInvariant();
            return _wordDictionary.BlackList.Any(bli => TestWord(testableString, bli));
        }

        private bool TestWord(string phrase, string badword)
        {
            return CultureInfo.CurrentCulture.CompareInfo.IndexOf(phrase, badword, CompareOptions.IgnoreCase) >= 0;
        }

        private string ReplaceWord(string phrase, string badword)
        {
            var pos = CultureInfo.CurrentCulture.CompareInfo.IndexOf(phrase, badword, CompareOptions.IgnoreCase);
            var result = phrase;
            while (pos >= 0)
            {
                var left = result.Substring(0, pos);
                var right = result.Substring(pos + badword.Length);
                result = $"{left}[{GetRandomReplacement()}]{right}";
                pos = CultureInfo.CurrentCulture.CompareInfo.IndexOf(result, badword, CompareOptions.IgnoreCase);
            }
            return result;
        }

        private string GetRandomReplacement()
        {
            if (!_wordDictionary.Replacements?.Any() ?? true) return "*****";
            var x = new Random();
            return _wordDictionary.Replacements[x.Next(_wordDictionary.Replacements.Length)];
        }

        public string ReplaceAll(string phrase)
        {
            var response = phrase;
            foreach (var badword in _wordDictionary.BlackList)
            {
                if (!TestWord(response, badword)) continue;
                response = ReplaceWord(response, badword);
            }
            return response;
        }
    }
}