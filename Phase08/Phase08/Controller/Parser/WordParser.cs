using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Phase08.Interfaces;


namespace Phase08
{
    public class WordParser : IParser<string>
    {
        private PorterStemmer _stemmer = new PorterStemmer();

        public string Parse(string text)
        {
            
            return _stemmer.StemWord(text);
        }

    }

  
}