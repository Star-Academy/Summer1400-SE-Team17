using System.Collections.Generic;
using System.Linq;
using Phase08.Interfaces;

namespace Phase08
{
    public class DocumentParser : IParser<string[]>
    {
        private IParser<string[]> _sentenceParser;

        public DocumentParser(ServiceFactory<IParser<string[]>> serviceFactory)
        {
            _sentenceParser = serviceFactory.GetService(typeof(DocumentParser));
        }

        public DocumentParser()
        {
        }

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
                parsedResult.UnionWith(_sentenceParser.Parse(sentence.Trim()));
            }
            
            return parsedResult.ToArray();
        }
    }

}