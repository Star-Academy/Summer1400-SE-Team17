using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Microsoft.Extensions.DependencyInjection;
using Phase08.Interfaces;

namespace Phase08
{
    public class SentenceParser : IParser<string[]>
    {
        private IParser<string> _wordParse;

        public SentenceParser(IParser<string> wordParse)
        {
            _wordParse = wordParse;
        }

        
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