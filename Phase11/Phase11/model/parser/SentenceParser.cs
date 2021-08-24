using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Phase11.model.parser
{
    public class SentenceParser : IParser<string[]>
    {
        private IParser<string> _wordParse = new DefaultWordParser();

        public IParser<string> WordParser
        {
            set => _wordParse = value;
        }

        public string[] Parse(string text)
        {
            text = text.ToLower();
            text = Regex.Replace(text , "[^a-z]" , " ");
            var words = text.Split(' ');
            var result = new HashSet<string>();
            foreach (var word in words)
            {
                result.Add(_wordParse.Parse(word));
            }
            result.Remove("");
            return result.ToArray();
        }
    }
}