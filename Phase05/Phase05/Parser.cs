
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Phase05;

namespace Parser
{
    public class WordParser : IParser<string>
    {
        private PorterStemmer _stemmer = new PorterStemmer();

        public string Parse(string text)
        {
            
            return _stemmer.StemWord(text);
        }

    }

    public class SentenceParser : IParser<string[]>
    {
        private IParser<string> _wordParse = new WordParser();

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

    public class DocumentParser : IParser<string[]>
    {
        private IParser<string[]> _sentenceParser = new SentenceParser();

        public IParser<string[]> SentenceParser
        {
            set => _sentenceParser = value;
        }

        public string[] Parse(string text)
        {
            var parsedResult = new HashSet<string>();
            var sentences = text.Split('.');
            foreach (var sentence in sentences)
            {
                parsedResult.UnionWith(_sentenceParser.Parse(sentence));
            }

            return parsedResult.ToArray();
        }
    }
}